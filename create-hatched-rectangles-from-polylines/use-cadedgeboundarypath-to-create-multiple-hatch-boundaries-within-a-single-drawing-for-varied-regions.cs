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







 static CadLwPolyline Rectangle(double x) {return new CadLwPolyline {ObjectHandle=Handle(),PointCount=4,Flag=CadLwPolylineFlag.Closed,Coordinates=new List<Cad2DPoint>{new Cad2DPoint(x,0),new Cad2DPoint(x+100,0),new Cad2DPoint(x+100,50),new Cad2DPoint(x,50)}};}



 static void VerifyHatches(CadImage saved,Dictionary<string,(double Angle,short Color,double Spacing)> expected) {
  foreach(var kv in expected){var h=saved.Entities.OfType<CadHatch>().Single(e=>e.ObjectHandle==kv.Key);Require(h.BoundaryPaths.Count>0&&h.NumberOfBoundaries==h.BoundaryPaths.Count,"Hatch boundaries did not persist.");Require(h.SoftOwner==Model(saved).BlockTableRecordHandle,"Hatch owner did not persist.");Require(Math.Abs(h.HatchAngle-kv.Value.Angle)<1e-8&&h.ColorId==kv.Value.Color,"Hatch angle/color did not persist.");Require(h.PatternDefinitions!=null&&h.PatternDefinitions.Count==1&&Math.Abs(h.PatternDefinitions[0].LineOffset.Y-kv.Value.Spacing)<1e-8,"Hatch line family did not persist.");}
 }

 static int Main(string[] args) {try {
string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");

 var expected=new Dictionary<string,(double Angle,short Color,double Spacing)>();string[] original;string[] removed=Array.Empty<string>();
 using(var cad=(CadImage)Image.Load(input)) {
  original=cad.Entities.Select(e=>e.ObjectHandle).ToArray();

  for(int i=0;i<2;i++) {var rect=Rectangle(800+i*150);var h=Hatch(Model(cad).BlockTableRecordHandle,800+i*150,4+i*3);h.HatchAngle=30+i*30;h.ColorId=(short)(1+i);Add(cad,rect,h);expected[h.ObjectHandle]=(h.HatchAngle,h.ColorId,4+i*3);}

  SaveCad(cad,"output/hatches.dxf");

 }
 using(var saved=(CadImage)Image.Load("output/hatches.dxf")) {VerifyHatches(saved,expected);var handles=saved.Entities.Select(e=>e.ObjectHandle).ToHashSet();Require(original.Where(h=>!removed.Contains(h)).All(handles.Contains)&&removed.All(h=>!handles.Contains(h)),"Hatch changes lost unrelated entities or retained replaced hatches.");Console.WriteLine("Verified persisted hatch configurations: "+expected.Count);}

Console.WriteLine("Verified task 45332");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
