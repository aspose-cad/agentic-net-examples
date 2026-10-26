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
using Aspose.CAD.FileFormats.Iges;
class Program {
 static void Require(bool condition, string message) { if(!condition) throw new InvalidOperationException(message); }
 static string Handle() { return "F"+Guid.NewGuid().ToString("N").Substring(0,14).ToUpperInvariant(); }
 static int Main(string[] args) {
  try {

   string inputPath="input.igs";
   if(!File.Exists(inputPath)) {Console.WriteLine("Input file not found: "+inputPath);return 0;}
   Directory.CreateDirectory("output");
   var load=new LoadOptions();
   using(var image=(IgesImage)Image.Load(inputPath,load)) {
    int entities=image.Entities.Count();
    string[] log={"Loaded: "+Path.GetFileName(inputPath),"Format model: "+image.GetType().FullName,"Source bytes: "+new FileInfo(inputPath).Length,"Format entities: "+entities,"Load diagnostics: "+load.Errors.Count};
    Require(entities>0,"Fixture contains no format entities.");
    File.WriteAllLines("output/load.log",log);foreach(string line in log) Console.WriteLine(line);
   }

   Console.WriteLine("Verified task 44447 with supplied drawing input.");

   return 0;
  } catch(Exception e) {Console.Error.WriteLine(e.Message);return 1;}
 }
}
