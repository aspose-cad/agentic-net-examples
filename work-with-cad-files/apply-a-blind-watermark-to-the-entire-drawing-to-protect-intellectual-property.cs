using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.WatermarkGuard;
class Program {
 const string Expected = "CAD"; // Set the owner's known expected payload, not an unknown-watermark detector.
 static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
 static bool Inputs(params string[] paths) { foreach (string path in paths) if (!File.Exists(path)) { Console.WriteLine("Input file not found: " + path); return false; } return true; }
 static string Hash(string path) { using (var hash=SHA256.Create()) using (var stream=File.OpenRead(path)) return BitConverter.ToString(hash.ComputeHash(stream)).Replace("-", ""); }
 static bool Supports(string path) { string ext=Path.GetExtension(path); return ext.Equals(".dwg",StringComparison.OrdinalIgnoreCase)||ext.Equals(".dxf",StringComparison.OrdinalIgnoreCase); }
 static bool Matches(string path, string expected) {
  using (Image image=Image.Load(path)) {
   Require(image is CadImage,"Only the verified DWG/DXF CAD model is supported by this sample.");
   image.WatermarkGuardService.TextToValidate=expected;
   var status=image.WatermarkGuardService.ValidationStatus();
   Console.WriteLine("Expected-payload validation: " + status);
   return status==WatermarkingStatus.SUCCESS;
  }
 }
 static void Verify(string path) {
  Require(Matches(path,Expected),"Expected watermark was not verified; do not publish this drawing. This does not prove absence of a different watermark.");
  Require(!Matches(path,Expected+"-incorrect"),"Incorrect payload was accepted.");
 }
 static void Embed(string input,string output) {
  Require(Supports(input),"This sample only verifies native DWG/DXF input; unsupported formats are not silently converted.");
  Require(!File.Exists(output),"Refusing to overwrite an existing output.");
  string pending=output+".pending.dxf";
  Require(!File.Exists(pending),"Pending output already exists.");
  string before=Hash(input);int count;string[] layouts;
  try {
   using (Image image=Image.Load(input)) {
    CadImage cad=image as CadImage;Require(cad!=null,"Input is not a DWG/DXF CAD model.");
    count=cad.Entities.Count();layouts=cad.Layouts.Values.Select(l=>l.LayoutName).OrderBy(n=>n).ToArray();
    image.WatermarkGuardService.TextToEmbed=Expected;
    image.Save(pending,new DxfOptions {OutputMode=CadOutputMode.Convert});
    Require(image.WatermarkGuardService.EmbeddingStatus()==WatermarkingStatus.SUCCESS,"Embedding was not confirmed after Save; output will not be published.");
   }
   Verify(pending);
   using (Image image=Image.Load(pending)) {
    CadImage cad=image as CadImage;Require(cad!=null,"Saved drawing has the wrong format model.");
    Require(cad.Entities.Count()==count,"Drawing entity count changed.");
    Require(cad.Layouts.Values.Select(l=>l.LayoutName).OrderBy(n=>n).SequenceEqual(layouts),"Drawing layouts changed.");
   }
   Require(Hash(input)==before,"Source drawing changed.");
   File.Move(pending,output);
   Console.WriteLine("Verified saved CAD asset: "+output);
  } finally { if(File.Exists(pending))File.Delete(pending); }
 }
 static int Main(string[] args) { try {

 string input="input.dwg";if(!Inputs(input))return 0;
 Directory.CreateDirectory("output");
 // The complete loaded drawing is saved; no entity/layer collection is filtered or replaced.
 Embed(input,"output/protected-entire-drawing.dxf");

 Console.WriteLine("Verified task 44796");return 0;
 }catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;}}
}
