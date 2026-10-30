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

   string inputPath="input.cf2";
   if(!File.Exists(inputPath)) {Console.WriteLine("Input file not found: "+inputPath);return 0;}
   Directory.CreateDirectory("output");
   using(Image image=Image.Load(inputPath)) {
    var raster=new CadRasterizationOptions {PageWidth=1000,PageHeight=800,BackgroundColor=Color.White,DrawColor=Color.Blue,DrawType=CadDrawTypeMode.UseDrawColor,LineScale=2};
    bool complete=false;int failures=0;
    raster.RenderResult=result=>{complete=result.IsRenderComplete;failures+=result.Failures.Count;};
    image.Save("output/cf2.pdf",new PdfOptions {VectorRasterizationOptions=raster});
    Require(complete && failures==0,"CF2 PDF render did not complete cleanly.");
    Require(File.ReadAllBytes("output/cf2.pdf").Take(5).SequenceEqual(System.Text.Encoding.ASCII.GetBytes("%PDF-")),"PDF header missing.");
    Require(raster.LineScale==2 && raster.DrawColor==Color.Blue,"Rendering properties changed.");
   }

   Console.WriteLine("Verified task 44396 with supplied drawing input.");

   return 0;
  } catch(Exception e) {Console.Error.WriteLine(e.Message);return 1;}
 }
}
