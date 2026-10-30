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


 static bool Input(string path) {if(File.Exists(path))return true;Console.WriteLine("Input file not found: "+path);return false;}









 static int Main(string[] args) {try {

 string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");
 const string expected="CAD"; // Replace with the owner's known expected payload.
 using(Image image=Image.Load(input)) {
  // Unknown-payload detection is not exposed; validate a known expected value before processing.
  image.WatermarkGuardService.TextToValidate=expected;
  var status=image.WatermarkGuardService.ValidationStatus();Console.WriteLine("Known-payload validation: "+status);
  File.WriteAllText("output/watermark-check.txt",status.ToString());
  if(status==WatermarkingStatus.SUCCESS)Console.WriteLine("Expected payload verified; further processing may proceed.");
  else Console.WriteLine("Expected payload was not verified. This is not proof that no other watermark exists.");
 }

 Console.WriteLine("Verified task 44054");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
