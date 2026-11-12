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
using Aspose.CAD.FileFormats.Tiff.Enums;

class Program {

 static void Require(bool ok,string message) {if(!ok)throw new InvalidOperationException(message);}

 static void SaveCad(CadImage cad,string path) {cad.Save(path,new DxfOptions {OutputMode=CadOutputMode.Convert});}

 static IEnumerable<CadLwPolyline> Polylines(CadImage cad) {return cad.Entities.Concat(cad.BlockEntities.Values.Cast<CadBlockEntity>().SelectMany(b=>b.Entities)).OfType<CadLwPolyline>().GroupBy(p=>p.ObjectHandle).Select(g=>g.First());}
 static string Geometry(CadLwPolyline p) {return p.Flag+"|"+p.Elevation+"|"+string.Join(";",p.Coordinates.Select(v=>v.X+","+v.Y))+"|"+string.Join(",",Enumerable.Range(0,p.Coordinates.Count).Select(i=>p.Bugle!=null&&i<p.Bugle.Count?p.Bugle[i]:0));}

 static int Main(string[] args) {try {

 string[] inputs=args.Length>0?args:new[]{"first.dwg","second.dxf"};if(!inputs.Any(File.Exists)&&args.Length==0){Console.WriteLine("Input file not found: first.dwg / second.dxf");return 0;}
 foreach(string file in inputs)Require(File.Exists(file),"Batch input is missing: "+file);Directory.CreateDirectory("output");
 for(int i=0;i<inputs.Length;i++) {
  string path="output/widths-"+i+".dxf";var expected=new Dictionary<string,string>();
  using(var cad=(CadImage)Image.Load(inputs[i])) {
   var polylines=Polylines(cad).ToArray();Require(polylines.Length>0,"Batch drawing has no lightweight polylines.");
   foreach(var p in polylines){expected[p.ObjectHandle]=Geometry(p);p.ConstantWidth=1.25;p.ColorId=2;}SaveCad(cad,path);
  }
  using(var saved=(CadImage)Image.Load(path)){var actual=Polylines(saved).ToDictionary(p=>p.ObjectHandle);foreach(var pair in expected){Require(actual.TryGetValue(pair.Key,out var p)&&Geometry(p)==pair.Value&&p.ConstantWidth==1.25&&p.ColorId==2,"Polyline properties or geometry changed unexpectedly.");}}
  Console.WriteLine("Verified consistent polyline properties for "+inputs[i]);
 }

 Console.WriteLine("Verified task 45224");return 0;
} catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;} }
}
