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




 static CadRasterImage AddRaster(CadImage cad,string rasterPath,double x=0,double y=0) {
  Require(File.Exists(rasterPath),"Raster source is missing: "+rasterPath);
  string owner=Model(cad).BlockTableRecordHandle;
  CadRasterImageDef definition;
  using(Image pixels=Image.Load(rasterPath)) definition=new CadRasterImageDef(rasterPath,pixels.Width,pixels.Height) {ObjectHandle=Handle(),SoftOwner=owner};
  // CadRasterImageDef is the CadBaseObject stored in Objects; image instances are entities.
  CadBaseObject definitionObject=definition;cad.Objects=cad.Objects.Concat(new[]{definitionObject}).ToArray();
  var image=new CadRasterImage(definition,new Cad3DPoint(x,y,0),new Cad3DPoint(1,0,0),new Cad3DPoint(0,1,0)) {ObjectHandle=Handle(),SoftOwner=owner,ImageDefReference=definition.ObjectHandle};
  Add(cad,image);return image;
 }

 static Cad3DPoint Down(Cad3DPoint p) {return p==null?null:new Cad3DPoint(p.X,p.Y-100,p.Z);}
 static Cad3DPoint[] Positions(CadEntityBase entity) {
  if(entity is CadLine line)return new[]{line.FirstPoint,line.SecondPoint};
  if(entity is CadCircle circle)return new[]{circle.CenterPoint};
  if(entity is CadText text)return new[]{text.FirstAlignment};
  if(entity is CadMText mtext)return new[]{mtext.InsertionPoint};
  if(entity is CadRasterImage image)return new[]{image.InsertionPoint};
  if(entity is CadLwPolyline polyline)return polyline.Coordinates.Select(p=>new Cad3DPoint(p.X,p.Y,polyline.Elevation)).ToArray();
  throw new NotSupportedException("Unsupported geometry position model.");
 }
 static void Move(CadEntityBase entity) {
  if(entity is CadLine line) {line.FirstPoint=Down(line.FirstPoint);line.SecondPoint=Down(line.SecondPoint);}
  else if(entity is CadCircle circle)circle.CenterPoint=Down(circle.CenterPoint);
  else if(entity is CadText text) {text.FirstAlignment=Down(text.FirstAlignment);text.SecondAlignmentPoint=Down(text.SecondAlignmentPoint);}
  else if(entity is CadMText mtext)mtext.InsertionPoint=Down(mtext.InsertionPoint);
  else if(entity is CadLwPolyline polyline) {Require(polyline.ExtrusionDirection==null||(Math.Abs(polyline.ExtrusionDirection.X)<1e-12&&Math.Abs(polyline.ExtrusionDirection.Y)<1e-12&&polyline.ExtrusionDirection.Z>0),"Non-XY polyline needs a coordinate-system transform.");polyline.Coordinates=polyline.Coordinates.Select(p=>new Cad2DPoint(p.X,p.Y-100)).ToList();}
  else if(entity is CadRasterImage image)image.InsertionPoint=Down(image.InsertionPoint);
 }

 static int Main(string[] args) {try {

 string input="input.dxf",rasterPath="background.png";if(!Input(input)||!Input(rasterPath))return 0;Directory.CreateDirectory("output");
 string titleHandle=Handle(),backgroundHandle;int before;string[] originalHandles;double titleY;
 Dictionary<string,Cad3DPoint[]> originalGeometry;
 using(var cad=(CadImage)Image.Load(input)) {
  // Validate the supported geometry before mutating anything. No Bounds-only transforms.
  Require(cad.Entities.All(e=>e is CadLine||e is CadCircle||e is CadText||e is CadMText||e is CadLwPolyline||e is CadRasterImage),"This translation sample needs a geometry implementation for one or more entity types. No drawing was saved.");
  before=cad.Entities.Count();originalHandles=cad.Entities.Select(e=>e.ObjectHandle).ToArray();
  originalGeometry=cad.Entities.ToDictionary(e=>e.ObjectHandle,e=>Positions(e).Select(p=>new Cad3DPoint(p.X,p.Y,p.Z)).ToArray());
  var background=AddRaster(cad,rasterPath);backgroundHandle=background.ObjectHandle;
  foreach(var entity in cad.Entities)Move(entity);
  cad.UpdateSize(true);titleY=cad.MaxPoint.Y+20;
  var title=new CadText {ObjectHandle=titleHandle,DefaultValue="Drawing title",TextHeight=8,FirstAlignment=new Cad3DPoint(0,titleY,0)};Add(cad,title);cad.UpdateSize(true);SaveCad(cad,"output/titled.dxf");
 }
 using(var saved=(CadImage)Image.Load("output/titled.dxf")) {
  var background=saved.Entities.OfType<CadRasterImage>().Single(r=>r.ObjectHandle==backgroundHandle);
  Require(background.InsertionPoint.Y==-100&&saved.Entities.OfType<CadText>().Any(t=>t.ObjectHandle==titleHandle&&t.FirstAlignment.Y==titleY),"Background shift or title did not persist.");
  Require(saved.Entities.Count()==before+2&&originalHandles.All(h=>saved.Entities.Any(e=>e.ObjectHandle==h)),"Original entities were lost.");
  foreach(var handle in originalHandles) {var old=originalGeometry[handle];var points=Positions(saved.Entities.Single(e=>e.ObjectHandle==handle));Require(points.Length==old.Length,"Geometry point count changed.");for(int i=0;i<points.Length;i++)Require(Math.Abs(points[i].X-old[i].X)<1e-8&&Math.Abs(points[i].Y-(old[i].Y-100))<1e-8&&Math.Abs(points[i].Z-old[i].Z)<1e-8,"Geometry translation did not persist.");}
 }

 Console.WriteLine("Verified task 45099");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
