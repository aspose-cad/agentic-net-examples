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









 static int Main(string[] args) {try {

 string input="input.dwg";if(!Input(input))return 0;Directory.CreateDirectory("output");
 using(var cad=(CadImage)Image.Load(input)) {
  var lines=new List<string>();int inserts=0;
  // Search each insert instance separately; repeated block names are not keys.
  foreach(var insert in cad.Entities.OfType<CadInsertObject>()) {
   inserts++;
   // ATTRIB entities are children of the insert. ATTDEF belongs to the block definition.
   foreach(var attribute in insert.ChildObjects.OfType<Aspose.CAD.FileFormats.Cad.CadObjects.AttEntities.CadAttrib>())
    lines.Add(insert.ObjectHandle+"/"+insert.Name+":"+attribute.DefinitionTagString+"="+attribute.DefaultText);
   // CadObjectAttribute is SDK metadata, a separate hierarchy, never a child entity cast.
   foreach(var metadata in insert.Attributes??new List<CadObjectAttribute>())
    lines.Add(insert.ObjectHandle+" metadata "+metadata.Name+"="+string.Join(",",metadata.AttributeValues.Select(v=>v.Value)));
  }
  Require(inserts>0&&lines.Count>0,"No insert attributes to demonstrate the search.");
  File.WriteAllLines("output/attributes.txt",lines);foreach(string line in lines)Console.WriteLine(line);
 }

 Console.WriteLine("Verified task 45897");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
