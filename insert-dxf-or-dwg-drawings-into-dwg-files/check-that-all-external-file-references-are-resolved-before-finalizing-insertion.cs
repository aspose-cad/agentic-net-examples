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








 static void ResolveReferences(CadImage cad) {
  foreach(var definition in cad.Objects.OfType<CadRasterImageDef>())Require(!string.IsNullOrEmpty(definition.FileName)&&File.Exists(definition.FileName),"Unresolved raster reference: "+definition.FileName);
  foreach(var block in cad.BlockEntities.Values.Cast<CadBlockEntity>())if(!string.IsNullOrWhiteSpace(block.XRefPathName))Require(File.Exists(block.XRefPathName),"Unresolved external drawing: "+block.XRefPathName);
  foreach(var definition in cad.Objects.OfType<CadUnderlayDefinition>())Require(!string.IsNullOrWhiteSpace(definition.UnderlayPathAndFileName)&&File.Exists(definition.UnderlayPathAndFileName),"Unresolved underlay: "+definition.UnderlayPathAndFileName);
 }

 static int Main(string[] args) {try {

 string input=args.Length>0?args[0]:"input.dwg";if(!Input(input))return 0;Directory.CreateDirectory("output");
 using(var cad=(CadImage)Image.Load(input)) {
  // Run this gate on the modified drawing before finalizing its insertion/export.
  ResolveReferences(cad);int references=0;
  var blocks=cad.BlockEntities.Values.Cast<CadBlockEntity>().ToDictionary(b=>b.Name,StringComparer.OrdinalIgnoreCase);
  foreach(var entity in cad.Entities.Concat(blocks.Values.SelectMany(b=>b.Entities)).Distinct()) {
   if(entity is CadInsertObject insert) {Require(blocks.ContainsKey(insert.Name),"Unresolved insert block: "+insert.Name);references++;}
   if(entity is CadRasterImage image) {var definition=cad.Objects.OfType<CadRasterImageDef>().SingleOrDefault(d=>d.ObjectHandle==image.ImageDefReference);Require(definition!=null&&File.Exists(definition.FileName),"Unresolved raster image definition.");references++;}
  }
  Require(references>0,"No inserted block or raster reference was available to validate.");
  File.WriteAllText("output/reference-check.txt","Verified references: "+references);Console.WriteLine("Verified references: "+references);
 }

 Console.WriteLine("Verified task 45763");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
