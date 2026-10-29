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









 static void Visit(CadImage cad,CadBlockEntity block,string path,HashSet<string> active,List<string> output,int depth) {
  Require(depth<256,"Block reference depth limit exceeded.");
  if(!active.Add(block.Name)) {Console.WriteLine("Cycle at "+path);return;}
  foreach(var entity in block.Entities) {
   output.Add(path+"/"+block.Name+"/"+entity.ObjectHandle+":"+entity.TypeName);
   if(entity is CadInsertObject insert) {
    var child=cad.BlockEntities.Values.Cast<CadBlockEntity>().FirstOrDefault(b=>b.Name.Equals(insert.Name,StringComparison.OrdinalIgnoreCase));
    Require(child!=null,"Unresolved block reference: "+insert.Name);Visit(cad,child,path+"/"+insert.ObjectHandle,active,output,depth+1);
   }
  }
  active.Remove(block.Name);
 }

 static int Main(string[] args) {try {

 string input="input.dwg";if(!Input(input))return 0;Directory.CreateDirectory("output");
 using(var cad=(CadImage)Image.Load(input)) {
  var extracted=new List<string>();
  foreach(CadLayout layout in cad.Layouts.Values) {
   var block=cad.BlockEntities.Values.Cast<CadBlockEntity>().FirstOrDefault(b=>b.CadBlockTableObject?.ObjectHandle==layout.BlockTableRecordHandle);
   if(block!=null)Visit(cad,block,layout.LayoutName,new HashSet<string>(StringComparer.OrdinalIgnoreCase),extracted,0);
  }
  Require(extracted.Count>0,"No block-contained entities were extracted.");File.WriteAllLines("output/block-entities.txt",extracted);
  foreach(string entry in extracted)Console.WriteLine(entry);
  Console.WriteLine("Coordinates remain in each block's local coordinate system; this traversal does not flatten insert transforms.");
 }

 Console.WriteLine("Verified task 44934");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
