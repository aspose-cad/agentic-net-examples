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


 static void Remove(CadImage cad,HashSet<CadEntityBase> selected) {
  foreach(var block in cad.BlockEntities.Values.Cast<CadBlockEntity>())foreach(var entity in block.Entities.Where(selected.Contains).ToArray())block.Entities.Remove(entity);
  cad.Entities=cad.Entities.Where(e=>!selected.Contains(e)).ToArray();
 }






 static bool IsRectangle(CadLwPolyline p) {
  if((p.Flag&CadLwPolylineFlag.Closed)==0||p.Coordinates==null||p.Coordinates.Count!=4||Math.Abs(p.Elevation)>1e-9||(p.Bugle!=null&&p.Bugle.Any(b=>Math.Abs(b)>1e-9)))return false;
  var c=p.Coordinates;for(int i=0;i<4;i++){double ax=c[(i+1)%4].X-c[i].X,ay=c[(i+1)%4].Y-c[i].Y,bx=c[(i+2)%4].X-c[(i+1)%4].X,by=c[(i+2)%4].Y-c[(i+1)%4].Y;if(ax*ax+ay*ay<1e-12||Math.Abs(ax*bx+ay*by)>1e-8*Math.Sqrt((ax*ax+ay*ay)*(bx*bx+by*by)))return false;}return true;
 }
 static bool SamePoint(Point2D boundaryPoint,Cad2DPoint rectanglePoint) {return Math.Abs(boundaryPoint.X-rectanglePoint.X)<1e-8&&Math.Abs(boundaryPoint.Y-rectanglePoint.Y)<1e-8;}
 static bool SameBoundary(CadHatch h,CadLwPolyline p) {
  if(h.SoftOwner!=p.SoftOwner||h.BoundaryPaths==null||h.BoundaryPaths.Count!=1)return false;
  var paths=h.BoundaryPaths[0].BoundaryPath;if(paths==null||paths.Count!=1)return false;
  var edges=new List<(Point2D A,Point2D B)>();
  if(paths[0] is CadEdgeBoundaryPath boundary&&boundary.Objects!=null&&boundary.Objects.Count==4&&boundary.Objects.All(e=>e is CadBoundaryPathLine))foreach(CadBoundaryPathLine boundaryLine in boundary.Objects)edges.Add((boundaryLine.FirstPoint,boundaryLine.SecondPoint));
  else if(paths[0] is CadPolylineBoundaryPath poly&&poly.IsClosed&&poly.Vertices!=null&&poly.Vertices.Count==4&&(poly.Bugles==null||poly.Bugles.All(b=>Math.Abs(b)<1e-9)))for(int i=0;i<4;i++)edges.Add((poly.Vertices[i],poly.Vertices[(i+1)%4]));
  else return false;
  return Enumerable.Range(0,4).All(i=>edges.Any(e=>(SamePoint(e.A,p.Coordinates[i])&&SamePoint(e.B,p.Coordinates[(i+1)%4]))||(SamePoint(e.B,p.Coordinates[i])&&SamePoint(e.A,p.Coordinates[(i+1)%4]))));
 }
 static void VerifyHatches(CadImage saved,Dictionary<string,(double Angle,short Color,double Spacing)> expected) {
  foreach(var kv in expected){var h=saved.Entities.OfType<CadHatch>().Single(e=>e.ObjectHandle==kv.Key);Require(h.BoundaryPaths.Count>0&&h.NumberOfBoundaries==h.BoundaryPaths.Count,"Hatch boundaries did not persist.");Require(h.SoftOwner==Model(saved).BlockTableRecordHandle,"Hatch owner did not persist.");Require(Math.Abs(h.HatchAngle-kv.Value.Angle)<1e-8&&h.ColorId==kv.Value.Color,"Hatch angle/color did not persist.");Require(h.PatternDefinitions!=null&&h.PatternDefinitions.Count==1&&Math.Abs(h.PatternDefinitions[0].LineOffset.Y-kv.Value.Spacing)<1e-8,"Hatch line family did not persist.");}
 }

 static int Main(string[] args) {try {
string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");

 var expected=new Dictionary<string,(double Angle,short Color,double Spacing)>();string[] original;string[] removed=Array.Empty<string>();
 using(var cad=(CadImage)Image.Load(input)) {
  original=cad.Entities.Select(e=>e.ObjectHandle).ToArray();

  var rectangles=cad.Entities.OfType<CadLwPolyline>().Where(p=>p.SoftOwner==Model(cad).BlockTableRecordHandle&&IsRectangle(p)).ToArray();Require(rectangles.Length>0,"No straight closed rectangle polylines to hatch.");
  // Replace only existing fills with matching boundary geometry and ownership.
  var fills=cad.Entities.OfType<CadHatch>().Where(h=>rectangles.Any(p=>SameBoundary(h,p))).Cast<CadEntityBase>().ToHashSet();removed=fills.Select(h=>h.ObjectHandle).ToArray();Remove(cad,fills);
  // Preserve outlines and unrelated geometry; add one new fill per recognized rectangle.
  foreach(var p in rectangles){var h=Hatch(Model(cad).BlockTableRecordHandle,0,4);var edge=(CadEdgeBoundaryPath)h.BoundaryPaths[0].BoundaryPath[0];for(int i=0;i<4;i++){var a=p.Coordinates[i];var b=p.Coordinates[(i+1)%4];var line=(CadBoundaryPathLine)edge.Objects[i];line.FirstPoint=new Point2D(a.X,a.Y);line.SecondPoint=new Point2D(b.X,b.Y);}Add(cad,h);expected[h.ObjectHandle]=(h.HatchAngle,h.ColorId,4);}

  SaveCad(cad,"output/hatches.dxf");

 }
 using(var saved=(CadImage)Image.Load("output/hatches.dxf")) {VerifyHatches(saved,expected);var handles=saved.Entities.Select(e=>e.ObjectHandle).ToHashSet();Require(original.Where(h=>!removed.Contains(h)).All(handles.Contains)&&removed.All(h=>!handles.Contains(h)),"Hatch changes lost unrelated entities or retained replaced hatches.");Console.WriteLine("Verified persisted hatch configurations: "+expected.Count);}

Console.WriteLine("Verified task 45313");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
