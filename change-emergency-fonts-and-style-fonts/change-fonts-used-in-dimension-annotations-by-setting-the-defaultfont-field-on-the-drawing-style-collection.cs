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
using Aspose.CAD.FileFormats.Cad.CadTables;
class Program {
 static void Require(bool condition, string message) { if(!condition) throw new InvalidOperationException(message); }
 static string Handle() { return "F"+Guid.NewGuid().ToString("N").Substring(0,14).ToUpperInvariant(); }
 static int Main(string[] args) {
  try {

   string inputPath="input.dwg";
   if(!File.Exists(inputPath)) {Console.WriteLine("Input file not found: "+inputPath);return 0;}
   Directory.CreateDirectory("output");
   var changed=new List<string>();int entities;
   using(var cad=(CadImage)Image.Load(inputPath)) {
    entities=cad.Entities.Count();cad.DefaultFont="Arial";
    // DefaultFont belongs to CadImage. Dimension text uses referenced text-style records.
    var references=cad.DimensionStyles.Values.Cast<CadDimensionStyleTable>().Select(d=>d.Dimtxsty).ToHashSet(StringComparer.OrdinalIgnoreCase);
    foreach(CadStyleTableObject style in cad.Styles) {
     if(references.Contains(style.ObjectHandle)||references.Contains(style.StyleName)) {style.PrimaryFontName="Arial";changed.Add(style.StyleName);}
    }
    Require(changed.Count>0,"No dimension text-style references matched.");
    Require(cad.DefaultFont=="Arial" && cad.Entities.Count()==entities,"Font fallback changed drawing entities.");
    cad.Save("output/dimension-font.dxf",new DxfOptions {OutputMode=CadOutputMode.Convert});
    cad.Save("output/dimension-font.png",new PngOptions {VectorRasterizationOptions=new CadRasterizationOptions {PageWidth=1000,PageHeight=800}});
   }
   using(var saved=(CadImage)Image.Load("output/dimension-font.dxf")) {
    Require(saved.Entities.Count()==entities,"Entity count changed.");
    foreach(string name in changed) Require(saved.Styles.Cast<CadStyleTableObject>().Single(s=>s.StyleName==name).PrimaryFontName=="Arial","Dimension text font did not persist.");
   }
   Console.WriteLine("Updated dimension text-style fonts: "+string.Join(",",changed));

   Console.WriteLine("Verified task 45241 with supplied drawing input.");

   return 0;
  } catch(Exception e) {Console.Error.WriteLine(e.Message);return 1;}
 }
}
