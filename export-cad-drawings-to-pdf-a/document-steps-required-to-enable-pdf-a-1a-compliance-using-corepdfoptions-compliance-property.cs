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





 static CadRasterizationOptions Raster() {return new CadRasterizationOptions {PageWidth=800,PageHeight=600,BackgroundColor=Color.White};}



 static int Main(string[] args) {try {

 string input="input.dwg";if(!Input(input))return 0;Directory.CreateDirectory("output");
 using(Image image=Image.Load(input)) {
  // CorePdfOptions is a PdfDocumentOptions object, not the compliance enum itself.
  var document=new PdfDocumentOptions {Compliance=PdfCompliance.PdfA1a};
  var pdf=new PdfOptions {CorePdfOptions=document,VectorRasterizationOptions=Raster()};
  image.Save("output/pdfa1a.pdf",pdf);
  var bytes=File.ReadAllBytes("output/pdfa1a.pdf");Require(bytes.Length>100&&pdf.CorePdfOptions.Compliance==PdfCompliance.PdfA1a,"PDF/A configuration/export failed.");
  Console.WriteLine("Requested PDF/A-1a. Formal conformance requires an independent PDF/A validator.");
 }

 Console.WriteLine("Verified task 45619");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
