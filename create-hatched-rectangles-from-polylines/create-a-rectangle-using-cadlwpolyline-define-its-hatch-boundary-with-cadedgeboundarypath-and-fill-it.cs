using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;

class Program {
 static void Require(bool condition, string message) { if(!condition) throw new InvalidOperationException(message); }
 static string Handle() { return "F"+Guid.NewGuid().ToString("N").Substring(0,14).ToUpperInvariant(); }
 static int Main(string[] args) {
  try {

   string inputPath=args.Length>0?args[0]:"input.dwg";
   if(!File.Exists(inputPath)) {Console.WriteLine("Input file not found: "+inputPath);return 0;}
   Directory.CreateDirectory("output");
   string hatchHandle=Handle(),owner;int count,expected;
   using(var cad=(CadImage)Image.Load(inputPath)) {
    count=cad.Entities.Count();owner=cad.Layouts.Values.First(l=>l.LayoutName.Equals("Model",StringComparison.OrdinalIgnoreCase)).BlockTableRecordHandle;
    var polyline=new CadLwPolyline {ObjectHandle=Handle(),SoftOwner=owner,PointCount=4,Flag=CadLwPolylineFlag.Closed,Coordinates=new List<Cad2DPoint>{new Cad2DPoint(0,0),new Cad2DPoint(200,0),new Cad2DPoint(200,100),new Cad2DPoint(0,100)}};cad.Entities=cad.Entities.Concat(new CadEntityBase[]{polyline}).ToArray();
    // This sample handles straight, closed lightweight-polyline segments only.
    Require(polyline.Bugle==null || polyline.Bugle.All(b=>Math.Abs(b)<1e-12),"Curved segments require arc boundary entities.");
    Require(Math.Abs(polyline.Elevation)<1e-12 && (polyline.ExtrusionDirection==null || (Math.Abs(polyline.ExtrusionDirection.X)<1e-12 && Math.Abs(polyline.ExtrusionDirection.Y)<1e-12 && polyline.ExtrusionDirection.Z>=0)),"Non-XY polyline planes require a transformed hatch boundary.");
    var coordinates=polyline.Coordinates;int edges=coordinates.Count;
    var edge=new CadEdgeBoundaryPath {NumberOfEdges=edges,EdgeTypes=Enumerable.Repeat((short)1,edges).ToList(),Objects=new List<ICadBoundaryPathEntity>()};
    for(int i=0;i<edges;i++) edge.Objects.Add(new CadBoundaryPathLine {FirstPoint=new Point2D(coordinates[i].X,coordinates[i].Y),SecondPoint=new Point2D(coordinates[(i+1)%edges].X,coordinates[(i+1)%edges].Y)});
    var boundary=new CadHatchBoundaryPathContainer {PathType=0,BoundaryPath=new List<ICadBoundaryPath>{edge},SourceBoundaryObjects=new List<string>()};
    var hatch=new CadHatch {ObjectHandle=hatchHandle,SoftOwner=owner,PatternName="SOLID",SolidFillFlag=1,ColorId=1,NumberOfBoundaries=1,HatchScaleOrSpacing=1,BoundaryPaths=new List<CadHatchBoundaryPathContainer>{boundary},PatternDefinitions=new List<CadHatchPatternData>()};
    cad.Entities=cad.Entities.Concat(new CadEntityBase[]{hatch}).ToArray();expected=cad.Entities.Count();
    cad.Save("output/hatch.dxf",new DxfOptions {OutputMode=CadOutputMode.Convert});
   }
   using(var saved=(CadImage)Image.Load("output/hatch.dxf")) {
    var hatch=saved.Entities.OfType<CadHatch>().Single(h=>h.ObjectHandle==hatchHandle);
    var edge=hatch.BoundaryPaths.Single().BoundaryPath.OfType<CadEdgeBoundaryPath>().Single();
    Require(saved.Entities.Count()==expected && expected>count && hatch.PatternName=="SOLID" && hatch.SolidFillFlag==1 && hatch.SoftOwner==owner && edge.NumberOfEdges==edge.Objects.Count,"Hatch pattern/edges/ownership did not persist.");
    var segments=edge.Objects.OfType<CadBoundaryPathLine>().ToArray();
    for(int i=0;i<segments.Length;i++) Require(segments[i].SecondPoint.X==segments[(i+1)%segments.Length].FirstPoint.X && segments[i].SecondPoint.Y==segments[(i+1)%segments.Length].FirstPoint.Y,"Hatch path is open.");
    saved.Save("output/hatch.png",new PngOptions {VectorRasterizationOptions=new CadRasterizationOptions {PageWidth=800,PageHeight=600,DrawType=CadDrawTypeMode.UseObjectColor}});
    Require(new FileInfo("output/hatch.png").Length>0,"Hatch preview missing.");
   }

   Console.WriteLine("Verified task 45327 with supplied drawing input.");

   return 0;
  } catch(Exception e) {Console.Error.WriteLine(e.Message);return 1;}
 }
}
