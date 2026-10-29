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



 static int Main(string[] args) {try {

 string input="input.dwg";if(!Input(input))return 0;Directory.CreateDirectory("output");
 using(Image image=Image.Load(input)) {
  var caps=new[]{LineCap.Flat,LineCap.Round,LineCap.Square};var scales=new[]{1f,2f,.5f};var log=new List<string>{"cap,line_scale,milliseconds,bytes"};
  for(int i=0;i<caps.Length;i++) {
   var raster=Raster();raster.LineScale=scales[i];raster.PenOptions=new PenOptions {StartCap=caps[i],EndCap=caps[i]};
   var clock=Stopwatch.StartNew();string path="output/pen-"+i+".png";image.Save(path,new PngOptions {VectorRasterizationOptions=raster});clock.Stop();
   long bytes=new FileInfo(path).Length;Require(bytes>0,"PNG output empty.");log.Add(caps[i]+","+scales[i]+","+clock.ElapsedMilliseconds+","+bytes);
  }
  File.WriteAllLines("output/pen-performance.csv",log);foreach(string line in log)Console.WriteLine(line);
 }

 Console.WriteLine("Verified task 45925");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
