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

 static void EnsureRenderComplete(bool complete,int failures) {Require(complete&&failures==0,"Render incomplete or callback reported failures.");}

 static int Main(string[] args) {try {
string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");

 long limit=args.Length>0?long.Parse(args[0],System.Globalization.CultureInfo.InvariantCulture):20L*1024*1024;Require(limit>0,"File-size limit must be positive.");
 using(var cad=(CadImage)Image.Load(input)) {
  Require(cad.Entities.OfType<CadHatch>().Any()&&cad.Entities.Any(e=>e is CadText||e is CadMText),"Input needs real text and hatch geometry for this quality example.");var raster=Raster();raster.Quality=new RasterizationQuality {Text=RasterizationQualityValue.High,Hatch=RasterizationQualityValue.High,Arc=RasterizationQualityValue.High};bool complete=false;int failures=0;raster.RenderResult=result=>{complete=result.IsRenderComplete;failures+=result.Failures.Count;};
  var options=new PdfOptions {CorePdfOptions=new PdfDocumentOptions {Compliance=PdfCompliance.PdfA1a},VectorRasterizationOptions=raster};cad.Save("output/high-quality.pdf",options);EnsureRenderComplete(complete,failures);
 }
 var bytes=File.ReadAllBytes("output/high-quality.pdf");Require(bytes.Take(5).SequenceEqual(System.Text.Encoding.ASCII.GetBytes("%PDF-")),"PDF signature missing.");Require(bytes.LongLength>0&&bytes.LongLength<=limit,"Configured PDF/A output exceeds the file-size limit.");Console.WriteLine("PDF/A-1a requested with high text/hatch quality; bytes="+bytes.LongLength+"; limit="+limit+". Independent PDF/A conformance certification is outside this example.");

 Console.WriteLine("Verified task 45612");return 0;
} catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;} }
}
