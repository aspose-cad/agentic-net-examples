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
using Aspose.CAD.FileFormats.CF2;
class Program {
 static void Require(bool condition, string message) { if(!condition) throw new InvalidOperationException(message); }
 static string Handle() { return "F"+Guid.NewGuid().ToString("N").Substring(0,14).ToUpperInvariant(); }
 static int Main(string[] args) {
  try {

   string inputPath="input.cf2";
   if(!File.Exists(inputPath)) {Console.WriteLine("Input file not found: "+inputPath);return 0;}
   Directory.CreateDirectory("output");
   var load=new LoadOptions();
   using(var cf2=(CF2Image)Image.Load(inputPath,load)) {
    Require(load.Errors.Count==0,"CF2 load reported diagnostics.");
    int main=cf2.MainSecton.DrawnElements.Count;
    int subs=cf2.SubSectons.Sum(section=>section.DrawnElements.Count);
    Require(main+subs>0,"CF2 has no drawing elements.");
    double width=cf2.Width,height=cf2.Height;bool complete=false;int failures=0;
    var raster=new CadRasterizationOptions {PageWidth=1000,PageHeight=800};
    raster.RenderResult=result=>{complete=result.IsRenderComplete;failures+=result.Failures.Count;};
    cf2.Save("output/validated.pdf",new PdfOptions {VectorRasterizationOptions=raster});
    Require(complete&&failures==0,"Renderer reported incomplete output or failures.");
    Require(cf2.MainSecton.DrawnElements.Count==main && cf2.SubSectons.Sum(section=>section.DrawnElements.Count)==subs && cf2.Width==width && cf2.Height==height,"Export changed source CF2 data.");
    Require(File.ReadAllBytes("output/validated.pdf").Take(5).SequenceEqual(System.Text.Encoding.ASCII.GetBytes("%PDF-")),"PDF signature missing.");
    cf2.Save("output/reference.svg",new SvgOptions {VectorRasterizationOptions=new CadRasterizationOptions {PageWidth=1000,PageHeight=800}});
    Console.WriteLine("CF2 baseline: main elements="+main+", sub-section elements="+subs+", size="+width+" x "+height);
    Console.WriteLine("PDF renderer and source-invariance checks passed. Compare the PDF visually with reference.svg; source element counts alone do not prove that every element appears in the PDF.");
   }

   Console.WriteLine("Verified task 44605 with supplied drawing input.");

   return 0;
  } catch(Exception e) {Console.Error.WriteLine(e.Message);return 1;}
 }
}
