using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;
using Aspose.CAD.FileFormats.Dwf;
using Aspose.CAD.FileFormats.Dwf.Whip.Objects;
using Aspose.CAD.FileFormats.Dwf.Whip.Objects.Drawable.Text;
class Program {
 static void Require(bool condition, string message) { if(!condition) throw new InvalidOperationException(message); }
 static string Handle() { return "F"+Guid.NewGuid().ToString("N").Substring(0,14).ToUpperInvariant(); }
 static int Main(string[] args) {
  try {

   string inputPath="input.dwf";
   if(!File.Exists(inputPath)) {Console.WriteLine("Input file not found: "+inputPath);return 0;}
   Directory.CreateDirectory("output");
   using(var dwf=(DwfImage)Image.Load(inputPath)) {
    var texts=dwf.Pages.SelectMany(p=>p.Entities.OfType<DwfWhipText>()).ToArray();
    Require(texts.Length>0,"DWF contains no text to substitute.");
    var originalText=texts.Select(t=>t.Text.AsciiString).ToArray();
    var originalPositions=texts.Select(t=>(t.Position.X,t.Position.Y)).ToArray();
    string source=texts.First(t=>t.Font?.Name?.Value!=null).Font.Name.Value.AsciiString;
    // Replace this mapping with known missing-font names and fonts installed on the rendering host.
    var FontSubstitution=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase) {{source,"Courier New"}};
    int changed=0;
    foreach(var text in texts) {
     string current=text.Font?.Name?.Value?.AsciiString;
     if(current!=null && FontSubstitution.TryGetValue(current,out string replacement)) {text.Font.Name.Value=new DwfString(replacement);changed++;}
    }
    Require(changed>0,"No font matched the substitution map.");
    Require(texts.Select(t=>t.Text.AsciiString).SequenceEqual(originalText) && texts.Select(t=>(t.Position.X,t.Position.Y)).SequenceEqual(originalPositions),"Text or positions changed.");
    Require(texts.All(t=>!string.Equals(t.Font?.Name?.Value?.AsciiString,source,StringComparison.OrdinalIgnoreCase)) || source=="Courier New","Mapped font was not replaced.");
    dwf.Save("output/substituted.tiff",new TiffOptions {VectorRasterizationOptions=new CadRasterizationOptions {PageWidth=1000,PageHeight=800}});
    byte[] header=File.ReadAllBytes("output/substituted.tiff");Require(header.Length>8 && ((header[0]==73&&header[1]==73)||(header[0]==77&&header[1]==77)),"TIFF output missing.");
    Console.WriteLine("Substituted "+changed+" DWF text fonts without changing strings/positions. Font installation and final appearance need review on the target rendering host.");
   }

   Console.WriteLine("Verified task 45986 with supplied drawing input.");

   return 0;
  } catch(Exception e) {Console.Error.WriteLine(e.Message);return 1;}
 }
}
