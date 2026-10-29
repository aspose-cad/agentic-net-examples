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
  var before=cad.Layouts.Values.ToDictionary(l=>l.LayoutName,l=>(l.MinExtents.X,l.MinExtents.Y,l.MaxExtents.X,l.MaxExtents.Y));
  var handles=cad.Entities.Select(e=>e.ObjectHandle).ToArray();
  var raster=Raster();raster.Quality=new RasterizationQuality {Arc=RasterizationQualityValue.High,Text=RasterizationQualityValue.High,Hatch=RasterizationQualityValue.High};Pdf(cad,"output/high-quality.pdf",raster);
  foreach(CadLayout layout in cad.Layouts.Values) Require(before[layout.LayoutName]==(layout.MinExtents.X,layout.MinExtents.Y,layout.MaxExtents.X,layout.MaxExtents.Y),"Layout extents changed during rendering.");
  Require(cad.Entities.Select(e=>e.ObjectHandle).SequenceEqual(handles),"Export changed drawing entities.");
  Console.WriteLine("Layout extents and entity identities preserved; raster page dimensions are configured separately.");
 }

 Console.WriteLine("Verified task 45743");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
