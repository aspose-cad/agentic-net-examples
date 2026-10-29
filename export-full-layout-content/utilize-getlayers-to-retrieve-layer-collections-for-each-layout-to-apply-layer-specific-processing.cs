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
 static void Pdf(Image image,string path,CadRasterizationOptions raster) {
  bool called=false,complete=false;int errors=0;
  raster.RenderResult=result=>{called=true;complete=result.IsRenderComplete;errors+=result.Failures.Count;foreach(var failure in result.Failures)Console.WriteLine("Render diagnostic: "+failure.RenderCode+" "+failure.Message);};
  image.Save(path,new PdfOptions {VectorRasterizationOptions=raster});
  Require(called&&complete&&errors==0,"PDF render callback did not confirm a complete render.");
  Require(File.ReadAllBytes(path).Take(5).SequenceEqual(System.Text.Encoding.ASCII.GetBytes("%PDF-")),"PDF header missing.");
 }


 static int Main(string[] args) {try {

 string input="input.dwg";if(!Input(input))return 0;Directory.CreateDirectory("output");
 using(var cad=(CadImage)Image.Load(input)) {
  var allLayers=cad.Layers.GetLayersNames();Require(allLayers.Count>0,"No drawing layers.");int index=0;
  foreach(CadLayout layout in cad.Layouts.Values) {
   // GetLayersNames is drawing-wide. Layout membership comes from ownership.
   var names=cad.Entities.Where(e=>e.SoftOwner==layout.BlockTableRecordHandle||e.LayoutTabName==layout.LayoutName).Select(e=>e.LayerName).Where(n=>!string.IsNullOrWhiteSpace(n)).Distinct().ToArray();
   Console.WriteLine(layout.LayoutName+": "+string.Join(",",names));
   if(names.Length==0)continue;
   var raster=Raster();raster.Layouts=new[]{layout.LayoutName};raster.Layers=names;Pdf(cad,"output/layout-"+index+++".pdf",raster);
  }
  Require(index>0,"No layout with entities was exported.");
 }

 Console.WriteLine("Verified task 45487");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
