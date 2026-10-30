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




 static void SaveCad(CadImage cad,string path) {cad.Save(path,new DxfOptions {OutputMode=CadOutputMode.Convert});}




 static int Main(string[] args) {try {

 string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");string[] handles;
 using(var cad=(CadImage)Image.Load(input)) {
  var hatches=cad.Entities.OfType<CadHatch>().ToArray();Require(hatches.Length>0,"No model hatches.");handles=hatches.Select(h=>h.ObjectHandle).ToArray();
  foreach(var hatch in hatches) {
   Require(hatch.PatternDefinitions!=null&&hatch.PatternDefinitions.Count>0,"Solid hatch has no line-family origin to adjust.");
   foreach(var pattern in hatch.PatternDefinitions)pattern.LineBasePoint=new Cad2DPoint(10,20);
  }
  SaveCad(cad,"output/hatch-origin.dxf");
 }
 using(var saved=(CadImage)Image.Load("output/hatch-origin.dxf")) {
  foreach(var handle in handles)Require(saved.Entities.OfType<CadHatch>().Single(h=>h.ObjectHandle==handle).PatternDefinitions.All(p=>p.LineBasePoint.X==10&&p.LineBasePoint.Y==20),"Pattern line-family origin did not persist.");
  Console.WriteLine("Updated each pattern line-family base point; seed points remain unchanged.");
 }

 Console.WriteLine("Verified task 45797");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
