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
 using(var cad=(CadImage)Image.Load(input)) {
  int index=0;
  foreach(CadLayerTable layer in cad.Layers) {
   if(string.IsNullOrEmpty(layer.Name))continue;
   var raster=Raster();raster.Layers=new[]{layer.Name};
   string path="output/layer-"+index+++".tiff";
   cad.Save(path,new TiffOptions(Aspose.CAD.FileFormats.Tiff.Enums.TiffExpectedFormat.TiffDeflateRgba) {VectorRasterizationOptions=raster});
   var header=File.ReadAllBytes(path);Require(header.Length>8&&((header[0]==73&&header[1]==73)||(header[0]==77&&header[1]==77)),"Invalid TIFF header.");
   Console.WriteLine("Exported layer "+layer.Name+" -> "+path);
  }
  Require(index>0,"No layers exported.");
 }

 Console.WriteLine("Verified task 45994");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
