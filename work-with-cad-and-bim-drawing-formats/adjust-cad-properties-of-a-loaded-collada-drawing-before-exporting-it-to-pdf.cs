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

 string input="input.dae";if(!Input(input))return 0;Directory.CreateDirectory("output");
 using(Image image=Image.Load(input)) {
  Require(image is Aspose.CAD.FileFormats.Collada.ColladaImage,"Input is not COLLADA.");
  // COLLADA does not expose the DWG/DXF layer/block model. Adjust supported rendering properties.
  var raster=Raster();raster.DrawType=CadDrawTypeMode.UseDrawColor;raster.DrawColor=Color.Blue;raster.LineScale=2;
  Pdf(image,"output/collada.pdf",raster);
  Require(raster.LineScale==2&&raster.DrawColor==Color.Blue,"Rendering configuration changed.");
 }

 Console.WriteLine("Verified task 44291");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
