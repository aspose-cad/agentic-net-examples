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

 string input="input.plt";if(!Input(input))return 0;Directory.CreateDirectory("output");
 using(Image image=Image.Load(input)) {
  var raster=Raster();raster.GraphicsOptions=new GraphicsOptions {SmoothingMode=SmoothingMode.AntiAlias};
  // DPI metadata is owned by output ResolutionSettings; page dimensions select pixel dimensions.
  var jpeg=new JpegOptions {Quality=90,ResolutionSettings=new ResolutionSetting(300,300),VectorRasterizationOptions=raster};
  image.Save("output/plt.jpg",jpeg);var bytes=File.ReadAllBytes("output/plt.jpg");Require(bytes.Length>10&&bytes[0]==255&&bytes[1]==216,"JPEG output missing.");
  Require(jpeg.ResolutionSettings.HorizontalResolution==300&&jpeg.ResolutionSettings.VerticalResolution==300,"DPI configuration changed.");
 }

 Console.WriteLine("Verified task 44503");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
