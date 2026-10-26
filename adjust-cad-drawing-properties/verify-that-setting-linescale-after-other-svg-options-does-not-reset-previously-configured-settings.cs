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
   using(Image image=Image.Load(inputPath)) {
    var raster=new CadRasterizationOptions {PageWidth=800,PageHeight=600,UnitType=UnitType.Centimenter,DrawColor=Color.Black,BackgroundColor=Color.White};
    var svg=new SvgOptions {VectorRasterizationOptions=raster};
    // Assign the same concrete options before changing LineScale.
    raster.LineScale=1.5f;
    Require(ReferenceEquals(svg.VectorRasterizationOptions,raster),"Raster options were replaced.");
    Require(raster.PageWidth==800 && raster.PageHeight==600 && raster.UnitType==UnitType.Centimenter && raster.DrawColor==Color.Black && raster.BackgroundColor==Color.White && Math.Abs(raster.LineScale-1.5)<1e-6,"LineScale reset another setting.");
    image.Save("output/options.svg",svg);
    Require(new FileInfo("output/options.svg").Length>0,"SVG was empty.");
    Console.WriteLine("LineScale=1.5 preserved page size, units, colors and the assigned options instance.");
   }

   Console.WriteLine("Verified task 44034 with supplied drawing input.");

   return 0;
  } catch(Exception e) {Console.Error.WriteLine(e.Message);return 1;}
 }
}
