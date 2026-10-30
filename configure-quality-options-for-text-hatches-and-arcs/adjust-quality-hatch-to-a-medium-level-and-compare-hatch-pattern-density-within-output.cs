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

 string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");
 using(var cad=(CadImage)Image.Load(input)) {
  var hatches=cad.Entities.OfType<CadHatch>().ToArray();Require(hatches.Length>0,"No hatches to compare.");
  var spacing=hatches.Select(h=>h.HatchScaleOrSpacing).ToArray();
  foreach(var level in new[]{RasterizationQualityValue.Low,RasterizationQualityValue.Medium,RasterizationQualityValue.High}) {
   var raster=Raster();raster.Quality=new RasterizationQuality {Hatch=level};
   string path="output/hatch-"+level+".png";cad.Save(path,new PngOptions {VectorRasterizationOptions=raster});
   Require(new FileInfo(path).Length>0,"Hatch preview empty.");Console.WriteLine("Hatch quality "+level+"; original pattern spacing preserved.");
  }
  Require(hatches.Select(h=>h.HatchScaleOrSpacing).SequenceEqual(spacing),"Rendering quality must not change hatch geometry/density.");
  Console.WriteLine("Compare the three previews for rendering differences; Quality.Hatch controls rendering fidelity rather than pattern spacing.");
 }

 Console.WriteLine("Verified task 45949");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
