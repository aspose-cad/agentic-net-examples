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
using Aspose.CAD.FileFormats.Tiff.Enums;

class Program {

 static void Require(bool ok,string message) {if(!ok)throw new InvalidOperationException(message);}
 static bool Input(string path) {if(File.Exists(path))return true;Console.WriteLine("Input file not found: "+path);return false;}
 static string Handle() {return "F"+Guid.NewGuid().ToString("N").Substring(0,14).ToUpperInvariant();}

 static void SaveCad(CadImage cad,string path) {cad.Save(path,new DxfOptions {OutputMode=CadOutputMode.Convert});}
 static CadRasterizationOptions Raster() {return new CadRasterizationOptions {PageWidth=800,PageHeight=600,BackgroundColor=Color.White};}
 static void Pdf(Image image,string path,CadRasterizationOptions raster) {
  bool called=false,complete=false;int errors=0;
  raster.RenderResult=result=>{called=true;complete=result.IsRenderComplete;errors+=result.Failures.Count;foreach(var failure in result.Failures)Console.WriteLine("Render diagnostic: "+failure.RenderCode+" "+failure.Message);};
  image.Save(path,new PdfOptions {VectorRasterizationOptions=raster});
  Require(called&&complete&&errors==0,"PDF render callback did not confirm a complete render.");
  Require(File.ReadAllBytes(path).Take(5).SequenceEqual(System.Text.Encoding.ASCII.GetBytes("%PDF-")),"PDF header missing.");
 }
 static CadHatch Hatch(string owner,double x,double spacing,bool solid=false) {
  var points=new[]{new Point2D(x,0),new Point2D(x+100,0),new Point2D(x+100,50),new Point2D(x,50)};
  var edges=new CadEdgeBoundaryPath {NumberOfEdges=4,EdgeTypes=new List<short>{1,1,1,1},Objects=new List<ICadBoundaryPathEntity>()};
  for(int i=0;i<4;i++)edges.Objects.Add(new CadBoundaryPathLine {FirstPoint=points[i],SecondPoint=points[(i+1)%4]});
  var pattern=new CadHatchPatternData {LineAngle=45,LineBasePoint=new Cad2DPoint(0,0),LineOffset=new Cad2DPoint(0,spacing),DashLengthCount=0,DashLengths=new List<double>()};
  return new CadHatch {ObjectHandle=Handle(),SoftOwner=owner,PatternName=solid?"SOLID":"USER",SolidFillFlag=(short)(solid?1:0),HatchPatternType=0,HatchScaleOrSpacing=1,NumberOfBoundaries=1,NumberOfPatternDefinitions=(short)(solid?0:1),PatternDefinitions=solid?new List<CadHatchPatternData>():new List<CadHatchPatternData>{pattern},BoundaryPaths=new List<CadHatchBoundaryPathContainer>{new CadHatchBoundaryPathContainer {PathType=0,BoundaryPath=new List<ICadBoundaryPath>{edges},SourceBoundaryObjects=new List<string>()}}};
 }

 static void AddTo(CadImage cad,CadLayout layout,CadEntityBase entity) {
  string owner=layout.BlockTableRecordHandle;var block=cad.BlockEntities.Values.Cast<CadBlockEntity>().Single(b=>b.CadBlockTableObject?.ObjectHandle==owner);
  entity.SoftOwner=owner;if(string.IsNullOrEmpty(entity.ObjectHandle))entity.ObjectHandle=Handle();block.AddEntity(entity);cad.Entities=cad.Entities.Concat(new[]{entity}).ToArray();
 }

 static IEnumerable<CadLwPolyline> Polylines(CadImage cad) {return cad.Entities.Concat(cad.BlockEntities.Values.Cast<CadBlockEntity>().SelectMany(b=>b.Entities)).OfType<CadLwPolyline>().GroupBy(p=>p.ObjectHandle).Select(g=>g.First());}

 static bool IsRectangle(CadLwPolyline p) {
  if((p.Flag&CadLwPolylineFlag.Closed)==0||p.Coordinates==null||p.Coordinates.Count!=4||Math.Abs(p.Elevation)>1e-9||(p.Bugle!=null&&p.Bugle.Any(b=>Math.Abs(b)>1e-9)))return false;
  for(int i=0;i<4;i++){var a=p.Coordinates[i];var b=p.Coordinates[(i+1)%4];var c=p.Coordinates[(i+2)%4];double x=b.X-a.X,y=b.Y-a.Y,u=c.X-b.X,v=c.Y-b.Y;if(x*x+y*y<1e-12||Math.Abs(x*u+y*v)>1e-8*Math.Sqrt((x*x+y*y)*(u*u+v*v)))return false;}return true;
 }
 static CadHatch HatchFrom(CadImage cad,CadLwPolyline outline,double spacing) {
  Require(IsRectangle(outline),"This example requires a straight closed XY rectangle.");var hatch=Hatch(outline.SoftOwner,0,spacing);var edge=(CadEdgeBoundaryPath)hatch.BoundaryPaths[0].BoundaryPath[0];
  for(int i=0;i<4;i++){var a=outline.Coordinates[i];var b=outline.Coordinates[(i+1)%4];var line=(CadBoundaryPathLine)edge.Objects[i];line.FirstPoint=new Point2D(a.X,a.Y);line.SecondPoint=new Point2D(b.X,b.Y);}return hatch;
 }
 static void ValidateHatch(CadHatch hatch,CadLwPolyline outline,double spacing) {
  Require(hatch.SoftOwner==outline.SoftOwner&&hatch.NumberOfBoundaries==1&&hatch.BoundaryPaths.Count==1,"Boundary ownership/count did not persist.");var container=hatch.BoundaryPaths.Single();Require(container.PathType==0,"Expected an edge boundary, not a polyline boundary.");var edge=(CadEdgeBoundaryPath)container.BoundaryPath.Single();Require(edge.NumberOfEdges==4&&edge.Objects.Count==4&&edge.EdgeTypes.All(t=>t==1),"Rectangle edge metadata did not persist.");
  for(int i=0;i<4;i++){var line=(CadBoundaryPathLine)edge.Objects[i];var a=outline.Coordinates[i];var b=outline.Coordinates[(i+1)%4];Require(Math.Abs(line.FirstPoint.X-a.X)<1e-8&&Math.Abs(line.FirstPoint.Y-a.Y)<1e-8&&Math.Abs(line.SecondPoint.X-b.X)<1e-8&&Math.Abs(line.SecondPoint.Y-b.Y)<1e-8,"Rectangle edge coordinates changed.");}
  var pattern=hatch.PatternDefinitions.Single();Require(hatch.PatternName=="USER"&&Math.Abs(pattern.LineOffset.Y-spacing)<1e-8&&Math.Abs(pattern.LineAngle-45)<1e-8,"Custom hatch line family did not persist.");
 }

 static int Main(string[] args) {try {
string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");

 double spacing=args.Length>0?double.Parse(args[0],System.Globalization.CultureInfo.InvariantCulture):6;Require(double.IsFinite(spacing)&&spacing>0,"Pattern spacing must be positive and finite.");string hatchHandle,outlineHandle;string[] original;
 using(var cad=(CadImage)Image.Load(input)) {
  var outline=cad.Entities.OfType<CadLwPolyline>().FirstOrDefault(IsRectangle);Require(outline!=null,"No straight closed rectangle found.");outlineHandle=outline.ObjectHandle;original=cad.Entities.Select(e=>e.ObjectHandle).ToArray();
  var hatch=HatchFrom(cad,outline,spacing);hatchHandle=hatch.ObjectHandle;CadLayout layout=cad.Layouts.Values.Single(l=>l.BlockTableRecordHandle==outline.SoftOwner);AddTo(cad,layout,hatch);SaveCad(cad,"output/pattern.dxf");Pdf(cad,"output/pattern.pdf",Raster());
 }
 using(var saved=(CadImage)Image.Load("output/pattern.dxf")) {var outline=Polylines(saved).Single(p=>p.ObjectHandle==outlineHandle);var hatch=saved.Entities.OfType<CadHatch>().Single(h=>h.ObjectHandle==hatchHandle);ValidateHatch(hatch,outline,spacing);Require(original.All(saved.Entities.Select(e=>e.ObjectHandle).Contains),"Existing drawing content was lost.");}

 Console.WriteLine("Verified task 45305");return 0;
} catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;} }
}
