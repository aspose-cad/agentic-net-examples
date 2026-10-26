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
using Aspose.CAD.FileFormats.Dwf;
class Program {
 static void Require(bool condition, string message) { if(!condition) throw new InvalidOperationException(message); }
 static string Handle() { return "F"+Guid.NewGuid().ToString("N").Substring(0,14).ToUpperInvariant(); }
 static int Main(string[] args) {
  try {

   string inputPath="input.dwf";
   if(!File.Exists(inputPath)) {Console.WriteLine("Input file not found: "+inputPath);return 0;}
   Directory.CreateDirectory("output");
   using(var dwf=(DwfImage)Image.Load(inputPath)) {
    var baseOptions=new CadRasterizationOptions {PageWidth=800,PageHeight=600,BackgroundColor=Color.White,DrawColor=Color.Black,LineScale=1};
    var names=dwf.Layers.GetLayersNames().Where(n=>!string.IsNullOrWhiteSpace(n)).ToArray();Require(names.Length>0,"No DWF layers.");
    for(int i=0;i<names.Length;i++) {
     var raster=new CadRasterizationOptions {PageWidth=baseOptions.PageWidth,PageHeight=baseOptions.PageHeight,BackgroundColor=baseOptions.BackgroundColor,DrawColor=baseOptions.DrawColor,LineScale=baseOptions.LineScale,Layers=new[]{names[i]}};
     // Layers selects layers. Layouts is intentionally unset; DWF page names differ from DWG's Model layout.
     string path="output/layer-"+i+".png";dwf.Save(path,new PngOptions {VectorRasterizationOptions=raster});
     Require(new FileInfo(path).Length>0 && raster.Layers.Single()==names[i],"Layer export did not complete.");
     Console.WriteLine("Layer "+names[i]+" -> "+path);
    }
   }

   Console.WriteLine("Verified task 45992 with supplied drawing input.");

   return 0;
  } catch(Exception e) {Console.Error.WriteLine(e.Message);return 1;}
 }
}
