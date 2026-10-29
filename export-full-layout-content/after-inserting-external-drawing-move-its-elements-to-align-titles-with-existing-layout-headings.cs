using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.FileFormats.Cad.CadObjects.UnderlayDefinition;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.Primitives;
using Aspose.CAD.WatermarkGuard;

class Program {

 static void Require(bool ok,string message) {if(!ok)throw new InvalidOperationException(message);}
 static bool Input(string path) {if(File.Exists(path))return true;Console.WriteLine("Input file not found: "+path);return false;}
 static string Handle() {return "F"+Guid.NewGuid().ToString("N").Substring(0,14).ToUpperInvariant();}
 static CadLayout Model(CadImage cad) {return cad.Layouts.Values.First(l=>l.LayoutName.Equals("Model",StringComparison.OrdinalIgnoreCase));}
 static CadBlockEntity ModelBlock(CadImage cad) {string owner=Model(cad).BlockTableRecordHandle;return cad.BlockEntities.Values.Cast<CadBlockEntity>().First(b=>b.CadBlockTableObject?.ObjectHandle==owner);}
 static void Add(CadImage cad,params CadEntityBase[] entities) {
  string owner=Model(cad).BlockTableRecordHandle;var block=ModelBlock(cad);
  foreach(var entity in entities) {entity.SoftOwner=owner;if(string.IsNullOrEmpty(entity.ObjectHandle))entity.ObjectHandle=Handle();block.AddEntity(entity);}
  cad.Entities=cad.Entities.Concat(entities).ToArray();
 }
 static void SaveCad(CadImage cad,string path) {cad.Save(path,new DxfOptions {OutputMode=CadOutputMode.Convert});}
 static CadRasterizationOptions Raster() {return new CadRasterizationOptions {PageWidth=800,PageHeight=600,BackgroundColor=Color.White};}
 static void Pdf(Image image,string path,CadRasterizationOptions raster) {
  bool called=false,complete=false;int errors=0;
  raster.RenderResult=result=>{called=true;complete=result.IsRenderComplete;errors+=result.Failures.Count;foreach(var failure in result.Failures)Console.WriteLine("Render diagnostic: "+failure.RenderCode+" "+failure.Message);};
  image.Save(path,new PdfOptions {VectorRasterizationOptions=raster});
  Require(called&&complete&&errors==0,"PDF render callback did not confirm a complete render.");
  Require(File.ReadAllBytes(path).Take(5).SequenceEqual(System.Text.Encoding.ASCII.GetBytes("%PDF-")),"PDF header missing.");
 }


 static Cad3DPoint Point(Cad3DPoint p) {return new Cad3DPoint(p.X,p.Y,p.Z);}
 static CadEntityBase Copy(CadEntityBase source) {
  CadEntityBase copy;
  if(source is CadLine line)copy=new CadLine(Point(line.FirstPoint),Point(line.SecondPoint));
  else if(source is CadCircle circle&&source is not CadArc)copy=new CadCircle {CenterPoint=Point(circle.CenterPoint),Radius=circle.Radius};
  else if(source is CadText text)copy=new CadText {DefaultValue=text.DefaultValue,FirstAlignment=Point(text.FirstAlignment),TextHeight=text.TextHeight,TextRotation=text.TextRotation};
  else if(source is CadMText mtext)copy=new CadMText {Text=mtext.Text,InsertionPoint=Point(mtext.InsertionPoint),InitialTextHeight=mtext.InitialTextHeight,ReferenceRectangleWidth=mtext.ReferenceRectangleWidth,RotationAngleRad=mtext.RotationAngleRad};
  else if(source is CadLwPolyline polyline)copy=new CadLwPolyline {Coordinates=polyline.Coordinates.Select(p=>new Cad2DPoint(p.X,p.Y)).ToList(),PointCount=polyline.Coordinates.Count,Flag=polyline.Flag,Elevation=polyline.Elevation,ExtrusionDirection=polyline.ExtrusionDirection==null?null:Point(polyline.ExtrusionDirection),Bugle=polyline.Bugle==null?null:new List<double>(polyline.Bugle)};
  else throw new NotSupportedException("Import sample supports lines, circles, TEXT, MTEXT and lightweight polylines; "+source.TypeName+" requires dependency-aware import. No output was saved.");
  copy.ObjectHandle=Handle();copy.LayerName=source.LayerName;copy.ColorId=source.ColorId;copy.ColorValue=source.ColorValue;copy.LineWeight=source.LineWeight;copy.LineScale=source.LineScale;copy.LineTypeName=source.LineTypeName;return copy;
 }
 static void ResolveReferences(CadImage cad) {
  foreach(var definition in cad.Objects.OfType<CadRasterImageDef>())Require(!string.IsNullOrEmpty(definition.FileName)&&File.Exists(definition.FileName),"Unresolved raster reference: "+definition.FileName);
  foreach(var block in cad.BlockEntities.Values.Cast<CadBlockEntity>())if(!string.IsNullOrWhiteSpace(block.XRefPathName))Require(File.Exists(block.XRefPathName),"Unresolved external drawing: "+block.XRefPathName);
  foreach(var definition in cad.Objects.OfType<CadUnderlayDefinition>())Require(!string.IsNullOrWhiteSpace(definition.UnderlayPathAndFileName)&&File.Exists(definition.UnderlayPathAndFileName),"Unresolved underlay: "+definition.UnderlayPathAndFileName);
 }

 static Cad3DPoint Offset(Cad3DPoint p,double x,double y,double z) {return new Cad3DPoint(p.X+x,p.Y+y,p.Z+z);}
 static void Translate(CadEntityBase entity,double x,double y,double z) {
  if(entity is CadLine line) {line.FirstPoint=Offset(line.FirstPoint,x,y,z);line.SecondPoint=Offset(line.SecondPoint,x,y,z);}
  else if(entity is CadCircle circle)circle.CenterPoint=Offset(circle.CenterPoint,x,y,z);
  else if(entity is CadText text)text.FirstAlignment=Offset(text.FirstAlignment,x,y,z);
  else if(entity is CadMText mtext)mtext.InsertionPoint=Offset(mtext.InsertionPoint,x,y,z);
  else if(entity is CadLwPolyline polyline) {Require(polyline.ExtrusionDirection==null||(Math.Abs(polyline.ExtrusionDirection.X)<1e-12&&Math.Abs(polyline.ExtrusionDirection.Y)<1e-12&&polyline.ExtrusionDirection.Z>0),"Non-XY polyline requires a coordinate-system transform.");polyline.Coordinates=polyline.Coordinates.Select(p=>new Cad2DPoint(p.X+x,p.Y+y)).ToList();polyline.Elevation+=z;}
 }

 static int Main(string[] args) {try {

 string input="input.dwg",external="external.dxf";if(!Input(input)||!Input(external))return 0;Directory.CreateDirectory("output");
 string[] originalHandles,addedHandles;double expectedX,expectedY;
 using(var cad=(CadImage)Image.Load(input))using(var source=(CadImage)Image.Load(external)) {
  ResolveReferences(cad);ResolveReferences(source);originalHandles=cad.Entities.Select(e=>e.ObjectHandle).ToArray();
  var heading=cad.Entities.OfType<CadText>().First();var title=source.Entities.OfType<CadText>().First();
  expectedX=heading.FirstAlignment.X;expectedY=heading.FirstAlignment.Y;
  double dx=expectedX-title.FirstAlignment.X,dy=expectedY-title.FirstAlignment.Y,dz=heading.FirstAlignment.Z-title.FirstAlignment.Z;
  string owner=Model(source).BlockTableRecordHandle;var sourceEntities=source.Entities.Where(e=>e.SoftOwner==owner).ToArray();
  var copies=sourceEntities.Select(Copy).ToArray();var layers=cad.Layers.GetLayersNames();
  Require(copies.All(e=>string.IsNullOrEmpty(e.LayerName)||layers.Contains(e.LayerName)),"External layer dependencies need explicit import.");
  foreach(var entity in copies)Translate(entity,dx,dy,dz);
  addedHandles=copies.Select(e=>e.ObjectHandle).ToArray();Add(cad,copies);cad.UpdateSize(true);SaveCad(cad,"output/aligned.dxf");
 }
 using(var saved=(CadImage)Image.Load("output/aligned.dxf")) {
  Require(originalHandles.All(h=>saved.Entities.Any(e=>e.ObjectHandle==h))&&saved.Entities.Count()==originalHandles.Length+addedHandles.Length,"Alignment lost drawing entities.");
  Require(saved.Entities.OfType<CadText>().Any(t=>addedHandles.Contains(t.ObjectHandle)&&Math.Abs(t.FirstAlignment.X-expectedX)<1e-8&&Math.Abs(t.FirstAlignment.Y-expectedY)<1e-8),"Imported title did not align with the target heading.");
  Pdf(saved,"output/aligned.pdf",Raster());
 }

 Console.WriteLine("Verified task 45493");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
