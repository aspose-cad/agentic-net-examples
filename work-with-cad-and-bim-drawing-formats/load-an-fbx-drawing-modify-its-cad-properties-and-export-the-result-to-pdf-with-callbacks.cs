using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
class Program {
 static int Main() {
  string inputPath="input.fbx",outputPath="output/rendered.pdf";
  if(!File.Exists(inputPath)) {Console.Error.WriteLine("Input file not found: "+inputPath);return 0;}
  Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputPath)));
  var loadOptions=new LoadOptions { IgnoreErrors=true };
  try { using(Image image=Image.Load(inputPath,loadOptions)) {
   foreach(RenderResult error in loadOptions.Errors) Console.Error.WriteLine($"Load: {error.RenderCode}: {error.Message}");
   var raster=new CadRasterizationOptions {
    PageWidth=1024,PageHeight=768,UnitType=UnitType.Millimeter,LineScale=2f,
    Margins=new Margins {Left=5,Right=5,Top=5,Bottom=5},BackgroundColor=Color.White
   };
   raster.RenderResult=(CadRenderResult result)=> {
    Console.WriteLine("Render complete: "+result.IsRenderComplete);
    foreach(RenderResult failure in result.Failures) Console.Error.WriteLine($"Render: {failure.RenderCode}: {failure.Message}");
   };
   // Configure the CAD rendering properties before exporting the loaded format.
   image.Save(outputPath,new PdfOptions { VectorRasterizationOptions=raster });
   Console.WriteLine("Exported: "+outputPath);
  } return 0;
  } catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;}
 }
}