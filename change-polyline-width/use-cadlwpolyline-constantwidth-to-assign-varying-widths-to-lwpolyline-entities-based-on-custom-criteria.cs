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
















 static int Main(string[] args) {try {
string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");

 var expected=new Dictionary<string,double>();
 using(var cad=(CadImage)Image.Load(input)) {var polylines=cad.Entities.Concat(cad.BlockEntities.Values.Cast<CadBlockEntity>().SelectMany(b=>b.Entities)).OfType<CadLwPolyline>().GroupBy(p=>p.ObjectHandle).Select(g=>g.First()).ToArray();Require(polylines.Length>0,"No lightweight polylines to modify.");foreach(var p in polylines){double previous=p.ConstantWidth;double width=(p.Coordinates?.Count??0)>5?1.5:0.5;p.ConstantWidth=width;expected[p.ObjectHandle]=width;Console.WriteLine("Width "+previous+" -> "+width);}SaveCad(cad,"output/widths.dxf");}
 using(var saved=(CadImage)Image.Load("output/widths.dxf")) {foreach(var kv in expected)Require(saved.Entities.Concat(saved.BlockEntities.Values.Cast<CadBlockEntity>().SelectMany(b=>b.Entities)).OfType<CadLwPolyline>().First(p=>p.ObjectHandle==kv.Key).ConstantWidth==kv.Value,"Width did not persist.");}

Console.WriteLine("Verified task 45208");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
