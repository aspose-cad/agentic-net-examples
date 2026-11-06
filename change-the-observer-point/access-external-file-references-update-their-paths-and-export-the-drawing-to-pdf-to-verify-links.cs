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




 static void SaveCad(CadImage cad,string path) {cad.Save(path,new DxfOptions {OutputMode=CadOutputMode.Convert});}
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

 string replacement=args.Length>0?args[0]:"background.png";if(!File.Exists(replacement))throw new FileNotFoundException("Replacement raster reference missing.",replacement);
 using(Image pixels=Image.Load(replacement)) {Require(pixels.Width>0&&pixels.Height>0,"Invalid replacement raster.");}
 using(var cad=(CadImage)Image.Load(input)) {
  // Raster references only. DWG XREF blocks and PDF/DGN underlays require their own definition types.
  var definitions=cad.Objects.OfType<CadRasterImageDef>().ToArray();Require(definitions.Length>0,"No raster references to update.");var linked=cad.Entities.OfType<CadRasterImage>().Select(e=>e.ImageDefReference).ToHashSet();int updated=0;
  foreach(var d in definitions.Where(d=>linked.Contains(d.ObjectHandle))){d.FileName=Path.GetFullPath(replacement);updated++;}Require(updated>0,"No linked raster definition found.");
  SaveCad(cad,"output/references.dxf");Pdf(cad,"output/references.pdf",Raster());
  using(var saved=(CadImage)Image.Load("output/references.dxf"))Require(saved.Objects.OfType<CadRasterImageDef>().Where(d=>linked.Contains(d.ObjectHandle)).All(d=>d.FileName==Path.GetFullPath(replacement)),"Replacement paths did not persist.");Console.WriteLine("Verified linked raster references: "+updated);
 }

Console.WriteLine("Verified task 45184");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
