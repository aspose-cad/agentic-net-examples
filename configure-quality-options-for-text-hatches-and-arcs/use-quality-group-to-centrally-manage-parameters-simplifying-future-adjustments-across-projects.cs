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
using Aspose.CAD.FileFormats.Tiff.Enums;

class Program {

 static void Require(bool ok,string message) {if(!ok)throw new InvalidOperationException(message);}
 static bool Input(string path) {if(File.Exists(path))return true;Console.WriteLine("Input file not found: "+path);return false;}

 static CadRasterizationOptions Raster() {return new CadRasterizationOptions {PageWidth=800,PageHeight=600,BackgroundColor=Color.White};}
 static void Pdf(Image image,string path,CadRasterizationOptions raster) {
  bool called=false,complete=false;int errors=0;
  raster.RenderResult=result=>{called=true;complete=result.IsRenderComplete;errors+=result.Failures.Count;foreach(var failure in result.Failures)Console.WriteLine("Render diagnostic: "+failure.RenderCode+" "+failure.Message);};
  image.Save(path,new PdfOptions {VectorRasterizationOptions=raster});
  Require(called&&complete&&errors==0,"PDF render callback did not confirm a complete render.");
  Require(File.ReadAllBytes(path).Take(5).SequenceEqual(System.Text.Encoding.ASCII.GetBytes("%PDF-")),"PDF header missing.");
 }

 static void EnsureRenderComplete(bool complete,int failures) {Require(complete&&failures==0,"Render incomplete or callback reported failures.");}

 static int Main(string[] args) {try {
string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");

 using(var cad=(CadImage)Image.Load(input)) {
  var before=cad.Entities.Select(e=>e.ObjectHandle).ToArray();var sharedQuality=new RasterizationQuality {Text=RasterizationQualityValue.High,Hatch=RasterizationQualityValue.High,Arc=RasterizationQualityValue.High};
  var pdfRaster=Raster();var pngRaster=Raster();pdfRaster.Quality=sharedQuality;pngRaster.Quality=sharedQuality;Require(object.ReferenceEquals(pdfRaster.Quality,pngRaster.Quality),"Quality configuration was not shared.");
  Pdf(cad,"output/quality.pdf",pdfRaster);bool complete=false;int failures=0;pngRaster.RenderResult=result=>{complete=result.IsRenderComplete;failures+=result.Failures.Count;};cad.Save("output/quality.png",new PngOptions {VectorRasterizationOptions=pngRaster});EnsureRenderComplete(complete,failures);
  using(Image rendered=Image.Load("output/quality.png"))Require(rendered.Width==800&&rendered.Height==600,"Unexpected image dimensions.");Require(before.SequenceEqual(cad.Entities.Select(e=>e.ObjectHandle)),"Quality configuration changed drawing entities.");
 }

 Console.WriteLine("Verified task 45965");return 0;
} catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;} }
}
