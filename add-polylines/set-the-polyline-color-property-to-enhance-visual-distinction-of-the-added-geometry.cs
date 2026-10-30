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

   string inputPath="input.dwg";
   if(!File.Exists(inputPath)) {Console.WriteLine("Input file not found: "+inputPath);return 0;}
   Directory.CreateDirectory("output");
   string handle=Handle(),owner; int count;
   using(var cad=(CadImage)Image.Load(inputPath)) {
    count=cad.Entities.Count();owner=cad.Layouts.Values.First(l=>l.LayoutName.Equals("Model",StringComparison.OrdinalIgnoreCase)).BlockTableRecordHandle;
    var polyline=new CadLwPolyline {ObjectHandle=handle,SoftOwner=owner,ColorId=5,PointCount=4,Flag=CadLwPolylineFlag.Closed,Coordinates=new List<Cad2DPoint>{new Cad2DPoint(0,0),new Cad2DPoint(100,0),new Cad2DPoint(100,100),new Cad2DPoint(0,100)}};
    Require(!cad.Entities.Any(e=>string.Equals(e.ObjectHandle,handle,StringComparison.OrdinalIgnoreCase)),"Handle collision.");
    cad.Entities=cad.Entities.Concat(new CadEntityBase[]{polyline}).ToArray();
    cad.Save("output/blue-polyline.dxf",new DxfOptions {OutputMode=CadOutputMode.Convert});
   }
   using(var saved=(CadImage)Image.Load("output/blue-polyline.dxf")) {
    var polyline=saved.Entities.OfType<CadLwPolyline>().Single(p=>p.ObjectHandle==handle);
    Require(saved.Entities.Count()==count+1 && polyline.ColorId==5 && polyline.SoftOwner==owner && polyline.Coordinates.Count==4 && (polyline.Flag&CadLwPolylineFlag.Closed)!=0,"Polyline color, geometry or ownership did not persist.");
    Require(polyline.Coordinates[2].X==100 && polyline.Coordinates[2].Y==100,"Vertex changed.");
   }

   Console.WriteLine("Verified task 44983 with supplied drawing input.");

   return 0;
  } catch(Exception e) {Console.Error.WriteLine(e.Message);return 1;}
 }
}
