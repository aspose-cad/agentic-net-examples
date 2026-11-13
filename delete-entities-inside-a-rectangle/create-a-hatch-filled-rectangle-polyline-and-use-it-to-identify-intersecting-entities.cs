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
using Aspose.CAD.FileFormats.Tiff.Enums;

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


 static CadHatch Hatch(string owner,double x,double spacing,bool solid=false) {
  var points=new[]{new Point2D(x,0),new Point2D(x+100,0),new Point2D(x+100,50),new Point2D(x,50)};
  var edges=new CadEdgeBoundaryPath {NumberOfEdges=4,EdgeTypes=new List<short>{1,1,1,1},Objects=new List<ICadBoundaryPathEntity>()};
  for(int i=0;i<4;i++)edges.Objects.Add(new CadBoundaryPathLine {FirstPoint=points[i],SecondPoint=points[(i+1)%4]});
  var pattern=new CadHatchPatternData {LineAngle=45,LineBasePoint=new Cad2DPoint(0,0),LineOffset=new Cad2DPoint(0,spacing),DashLengthCount=0,DashLengths=new List<double>()};
  return new CadHatch {ObjectHandle=Handle(),SoftOwner=owner,PatternName=solid?"SOLID":"USER",SolidFillFlag=(short)(solid?1:0),HatchPatternType=0,HatchScaleOrSpacing=1,NumberOfBoundaries=1,NumberOfPatternDefinitions=(short)(solid?0:1),PatternDefinitions=solid?new List<CadHatchPatternData>():new List<CadHatchPatternData>{pattern},BoundaryPaths=new List<CadHatchBoundaryPathContainer>{new CadHatchBoundaryPathContainer {PathType=0,BoundaryPath=new List<ICadBoundaryPath>{edges},SourceBoundaryObjects=new List<string>()}}};
 }
 static bool Segment(double ax,double ay,double bx,double by) {
  double lo=0,hi=1,dx=bx-ax,dy=by-ay;
  double[] p={-dx,dx,-dy,dy},q={ax,100-ax,ay,50-ay};
  for(int i=0;i<4;i++) {if(Math.Abs(p[i])<1e-12) {if(q[i]<0)return false;}else {double t=q[i]/p[i];if(p[i]<0)lo=Math.Max(lo,t);else hi=Math.Min(hi,t);if(lo>hi)return false;}}
  return true;
 }
 static bool Intersects(CadImage cad,CadEntityBase entity) {
  if(entity is CadLine line)return Segment(line.FirstPoint.X,line.FirstPoint.Y,line.SecondPoint.X,line.SecondPoint.Y);
  if(entity is CadCircle circle && entity is not CadArc) {
   double x=circle.CenterPoint.X,y=circle.CenterPoint.Y,r=circle.Radius;
   double near=Math.Pow(x-Math.Clamp(x,0,100),2)+Math.Pow(y-Math.Clamp(y,0,50),2);
   double far=new[]{(0d,0d),(100d,0d),(100d,50d),(0d,50d)}.Max(p=>Math.Pow(x-p.Item1,2)+Math.Pow(y-p.Item2,2));
   return near<=r*r&&far>=r*r;
  }
  if(entity is CadLwPolyline polyline && (polyline.Bugle==null||polyline.Bugle.All(b=>Math.Abs(b)<1e-12)) && Math.Abs(polyline.Elevation)<1e-12) {
   var points=polyline.Coordinates;int segments=(polyline.Flag&CadLwPolylineFlag.Closed)!=0?points.Count:points.Count-1;
   for(int i=0;i<segments;i++)if(Segment(points[i].X,points[i].Y,points[(i+1)%points.Count].X,points[(i+1)%points.Count].Y))return true;return false;
  }
  cad.GetBounds(entity);var bounds=entity.Bounds;
  if(bounds!=null&&bounds.Count>0&&(bounds.Max(p=>p.X)<0||bounds.Min(p=>p.X)>100||bounds.Max(p=>p.Y)<0||bounds.Min(p=>p.Y)>50))return false;
  throw new NotSupportedException("Exact rectangle intersection needs an implementation for "+entity.TypeName+"; drawing was not saved.");
 }





 static CadLwPolyline Rectangle(double x) {return new CadLwPolyline {ObjectHandle=Handle(),PointCount=4,Flag=CadLwPolylineFlag.Closed,Coordinates=new List<Cad2DPoint>{new Cad2DPoint(x,0),new Cad2DPoint(x+100,0),new Cad2DPoint(x+100,50),new Cad2DPoint(x,50)}};}





 static int Main(string[] args) {try {
string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");

 string[] kept,selectedHandles;string rectangleHandle,hatchHandle;
 using(var cad=(CadImage)Image.Load(input)) {
  var original=cad.Entities.ToArray();
  // Exact XY intersection for lines, circles and straight coplanar LWPOLYLINEs.
  // The broad-phase bounds test only excludes disjoint unsupported types; overlapping unsupported types fail before save.
  var selected=original.Where(e=>Intersects(cad,e)).ToHashSet();selectedHandles=selected.Select(e=>e.ObjectHandle).ToArray();kept=original.Where(e=>!selected.Contains(e)).Select(e=>e.ObjectHandle).ToArray();

  var rectangle=Rectangle(0);var hatch=Hatch(Model(cad).BlockTableRecordHandle,0,4,true);Add(cad,rectangle,hatch);rectangleHandle=rectangle.ObjectHandle;hatchHandle=hatch.ObjectHandle;
  Console.WriteLine("Selected intersecting original entities: "+selected.Count);SaveCad(cad,"output/selection.dxf");
 }
 using(var saved=(CadImage)Image.Load("output/selection.dxf")) {var handles=saved.Entities.Select(e=>e.ObjectHandle).ToHashSet();Require(kept.All(handles.Contains)&&handles.Contains(rectangleHandle)&&handles.Contains(hatchHandle),"Selection operation lost unrelated geometry or its hatch-filled rectangle.");
  Require(selectedHandles.All(handles.Contains),"Identify-only operation deleted selected entities.");
 }

Console.WriteLine("Verified task 45344");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
