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

   string inputPath="input.plt";
   if(!File.Exists(inputPath)) {Console.WriteLine("Input file not found: "+inputPath);return 0;}
   Directory.CreateDirectory("output");
   var load=new LoadOptions();
   Console.WriteLine("Loading PLT: "+Path.GetFileName(inputPath));
   using(Image image=Image.Load(inputPath,load)) {
    string[] log={"Loaded PLT model: "+image.GetType().FullName,"Source bytes: "+new FileInfo(inputPath).Length,"Load diagnostics: "+load.Errors.Count};
    File.WriteAllLines("output/plt-load.log",log);foreach(string line in log)Console.WriteLine(line);
    // Logging does not require a DWG/DXF cast or an invented Model layout.
   }

   Console.WriteLine("Verified task 44448 with supplied drawing input.");

   return 0;
  } catch(Exception e) {Console.Error.WriteLine(e.Message);return 1;}
 }
}
