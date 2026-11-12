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

 static string[] SelectLayers(IEnumerable<CadLayerTable> layers) {return layers.Where(l=>l.ColorId>=0&&(l.Flags&1)==0&&!l.Name.Equals("TEMP",StringComparison.OrdinalIgnoreCase)).Select(l=>l.Name).ToArray();}
 static void EnsureRenderComplete(bool complete,int failures) {Require(complete&&failures==0,"Render incomplete or callback reported failures.");}

 static int Main(string[] args) {try {
string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");

 using(var cad=(CadImage)Image.Load(input)) {
  var layers=SelectLayers(cad.Layers.Cast<CadLayerTable>());Require(layers.Length>0,"No visible non-excluded layers to export; refusing an all-layer fallback.");var raster=Raster();raster.Layers=layers;bool complete=false;int failures=0;
  raster.RenderResult=result=>{complete=result.IsRenderComplete;failures+=result.Failures.Count;foreach(var failure in result.Failures)Console.Error.WriteLine("Render diagnostic: "+failure.RenderCode+" "+failure.Message);};
  cad.Save("output/visible-layers.pdf",new PdfOptions {VectorRasterizationOptions=raster});EnsureRenderComplete(complete,failures);Require(File.ReadAllBytes("output/visible-layers.pdf").Take(5).SequenceEqual(System.Text.Encoding.ASCII.GetBytes("%PDF-")),"PDF signature missing.");Console.WriteLine("Exported visible layers: "+string.Join(",",layers));
 }

 Console.WriteLine("Verified task 45379");return 0;
} catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;} }
}
