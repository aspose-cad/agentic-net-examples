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

 string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");
 using(var cad=(CadImage)Image.Load(input)) {
  int before=cad.Entities.Count(),changed=0;
  foreach(var line in cad.Entities.OfType<CadLine>()) {
   var a=line.FirstPoint;var b=line.SecondPoint;line.FirstPoint=new Cad3DPoint(a.X+1,a.Y+1,a.Z);line.SecondPoint=new Cad3DPoint(b.X+1,b.Y+1,b.Z);line.ColorId=1;changed++;
  }
  Require(changed>0&&cad.Entities.Count()==before,"No line geometry was modified or other entities were lost.");cad.UpdateSize(true);Pdf(cad,"output/modified.pdf",Raster());Console.WriteLine("Shifted "+changed+" line entities.");
 }

 Console.WriteLine("Verified task 44629");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
