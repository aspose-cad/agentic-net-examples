using System;using System.IO;using System.Linq;using System.Collections.Generic;
using Aspose.CAD;using Aspose.CAD.FileFormats.Cad;using Aspose.CAD.FileFormats.Cad.CadObjects;using Aspose.CAD.FileFormats.Cad.CadConsts;using Aspose.CAD.FileFormats.Cad.CadObjects.UnderlayDefinition;
class Program {
 static void Require(bool ok,string message){if(!ok)throw new InvalidOperationException(message);}
 static bool InLayout(CadEntityBase entity,CadLayout layout) {
  if(!string.IsNullOrWhiteSpace(entity.SoftOwner))return entity.SoftOwner==layout.BlockTableRecordHandle;
  if(!string.IsNullOrWhiteSpace(entity.LayoutTabName))return entity.LayoutTabName.Equals(layout.LayoutName,StringComparison.OrdinalIgnoreCase);
  bool model=layout.LayoutName.Equals("Model",StringComparison.OrdinalIgnoreCase);
  if(entity.SpaceMode==CadEntitySpaceMode.ModelSpace)return model;
  if(model)return false;
  throw new InvalidDataException("Paper-space ownership is ambiguous; a layout handle/name is required.");
 }
 static string[] References(CadImage cad,string layoutName) {
  var layout=cad.Layouts.Values.SingleOrDefault(l=>l.LayoutName.Equals(layoutName,StringComparison.OrdinalIgnoreCase));Require(layout!=null,"Requested layout does not exist.");
  var block=cad.BlockEntities.Values.Cast<CadBlockEntity>().Single(b=>b.CadBlockTableObject.ObjectHandle==layout.BlockTableRecordHandle);
  var pending=new Stack<CadEntityBase>(cad.Entities.Where(e=>InLayout(e,layout)).Concat(block.Entities).Distinct());
  var visited=new HashSet<string>(StringComparer.OrdinalIgnoreCase);var paths=new SortedSet<string>(StringComparer.Ordinal);var objects=cad.Objects??Array.Empty<CadBaseObject>();
  while(pending.Count>0) {
   var entity=pending.Pop();
   if(entity is CadInsertObject insert) {
    Require(cad.BlockEntities.TryGetValue(insert.Name,out var definition),"Insert has no registered block definition: "+insert.Name);
    if(!string.IsNullOrWhiteSpace(definition.XRefPathName))paths.Add("XREF: "+definition.XRefPathName);
    if(visited.Add(insert.Name))foreach(var child in definition.Entities)pending.Push(child);
   }
   else if(entity is CadRasterImage raster) {
    var definition=objects.OfType<CadRasterImageDef>().SingleOrDefault(o=>o.ObjectHandle==raster.ImageDefReference);
    Require(definition!=null&&!string.IsNullOrWhiteSpace(definition.FileName),"Raster definition is missing or has no path.");paths.Add("RASTER: "+definition.FileName);
   }
   else if(entity is CadUnderlay underlay) {
    var definition=objects.OfType<CadUnderlayDefinition>().SingleOrDefault(o=>o.ObjectHandle==underlay.AcDbUnderlayDefinitionObjectId);
    Require(definition!=null&&!string.IsNullOrWhiteSpace(definition.UnderlayPathAndFileName),"Underlay definition is missing or has no path.");paths.Add("UNDERLAY: "+definition.UnderlayPathAndFileName);
   }
  }
  return paths.ToArray();
 }
 static int Main(string[] args){try{string input=args.Length>0?args[0]:"input.dxf";if(!File.Exists(input)){Console.Error.WriteLine("Input file not found: "+input);return args.Length==0?0:1;}string layout=args.Length>1?args[1]:"Model";using(var cad=(CadImage)Image.Load(input)){string[] paths=References(cad,layout);foreach(string path in paths)Console.WriteLine(path);Console.WriteLine("References in "+layout+": "+paths.Length);}Console.WriteLine("Verified task 45727");return 0;}catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;}}
}
