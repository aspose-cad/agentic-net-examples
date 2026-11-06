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
using Aspose.CAD.FileFormats.Tiff.Enums;

class Program {

 static void Require(bool ok,string message) {if(!ok)throw new InvalidOperationException(message);}
 static bool Input(string path) {if(File.Exists(path))return true;Console.WriteLine("Input file not found: "+path);return false;}





 static CadRasterizationOptions Raster() {return new CadRasterizationOptions {PageWidth=800,PageHeight=600,BackgroundColor=Color.White};}















 static int Main(string[] args) {try {
string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");

 using(Image image=Image.Load(input)) {
  var raster=Raster();Require(raster.PageWidth>0&&raster.PageHeight>0,"TIFF page size must be positive.");
  var options=new TiffOptions(TiffExpectedFormat.TiffLzwRgb) {Compression=TiffCompressions.Lzw,VectorRasterizationOptions=raster};
  // The active SDK reports save failures through its exceptions; do not invent ImageSaveException.
  try {image.Save("output/drawing.tiff",options);}catch(Exception ex){throw new InvalidOperationException("TIFF save failed with positive page dimensions.",ex);}
 }
 using(Image rendered=Image.Load("output/drawing.tiff"))Require(rendered.Width==800&&rendered.Height==600,"TIFF dimensions differ from configured page size.");

Console.WriteLine("Verified task 45996");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
