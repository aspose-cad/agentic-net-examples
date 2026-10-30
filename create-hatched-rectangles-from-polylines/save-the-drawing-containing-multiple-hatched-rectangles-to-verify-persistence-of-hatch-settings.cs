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

 static CadHatch Hatch(string owner,double x,double spacing,bool solid=false) {
  var points=new[]{new Point2D(x,0),new Point2D(x+100,0),new Point2D(x+100,50),new Point2D(x,50)};
  var edges=new CadEdgeBoundaryPath {NumberOfEdges=4,EdgeTypes=new List<short>{1,1,1,1},Objects=new List<ICadBoundaryPathEntity>()};
  for(int i=0;i<4;i++)edges.Objects.Add(new CadBoundaryPathLine {FirstPoint=points[i],SecondPoint=points[(i+1)%4]});
  var pattern=new CadHatchPatternData {LineAngle=45,LineBasePoint=new Cad2DPoint(0,0),LineOffset=new Cad2DPoint(0,spacing),DashLengthCount=0,DashLengths=new List<double>()};
  return new CadHatch {ObjectHandle=Handle(),SoftOwner=owner,PatternName=solid?"SOLID":"USER",SolidFillFlag=(short)(solid?1:0),HatchPatternType=0,HatchScaleOrSpacing=1,NumberOfBoundaries=1,NumberOfPatternDefinitions=(short)(solid?0:1),PatternDefinitions=solid?new List<CadHatchPatternData>():new List<CadHatchPatternData>{pattern},BoundaryPaths=new List<CadHatchBoundaryPathContainer>{new CadHatchBoundaryPathContainer {PathType=0,BoundaryPath=new List<ICadBoundaryPath>{edges},SourceBoundaryObjects=new List<string>()}}};
 }

 static int Main(string[] args) {try {

 string input="input.dwg";if(!Input(input))return 0;Directory.CreateDirectory("output");string first,second;int count;
 using(var cad=(CadImage)Image.Load(input)) {
  count=cad.Entities.Count();string owner=Model(cad).BlockTableRecordHandle;
  var a=Hatch(owner,0,5);var b=Hatch(owner,150,10);first=a.ObjectHandle;second=b.ObjectHandle;
  Add(cad,a,b);SaveCad(cad,"output/patterns.dxf");
 }
 using(var saved=(CadImage)Image.Load("output/patterns.dxf")) {
  Require(saved.Entities.Count()==count+2,"Existing entities were lost.");
  var a=saved.Entities.OfType<CadHatch>().Single(h=>h.ObjectHandle==first);var b=saved.Entities.OfType<CadHatch>().Single(h=>h.ObjectHandle==second);
  Require(a.PatternName=="USER"&&b.PatternName=="USER"&&a.SolidFillFlag==0&&b.SolidFillFlag==0,"Patterned hatches did not persist.");
  Require(Math.Abs(a.PatternDefinitions.Single().LineOffset.Y-5)<1e-8&&Math.Abs(b.PatternDefinitions.Single().LineOffset.Y-10)<1e-8,"Pattern spacing did not persist.");
  Require(a.BoundaryPaths.Single().BoundaryPath.OfType<CadEdgeBoundaryPath>().Single().Objects.Count==4,"Rectangle hatch edges lost.");
  saved.Save("output/patterns.png",new PngOptions {VectorRasterizationOptions=Raster()});
 }

 Console.WriteLine("Verified task 45336");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
