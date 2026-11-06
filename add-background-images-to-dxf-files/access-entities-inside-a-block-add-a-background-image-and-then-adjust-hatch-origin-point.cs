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
 static string Handle() {return "F"+Guid.NewGuid().ToString("N").Substring(0,14).ToUpperInvariant();}
 static CadLayout Model(CadImage cad) {return cad.Layouts.Values.First(l=>l.LayoutName.Equals("Model",StringComparison.OrdinalIgnoreCase));}
 static CadBlockEntity ModelBlock(CadImage cad) {string owner=Model(cad).BlockTableRecordHandle;return cad.BlockEntities.Values.Cast<CadBlockEntity>().First(b=>b.CadBlockTableObject?.ObjectHandle==owner);}
 static void Add(CadImage cad,params CadEntityBase[] entities) {
  string owner=Model(cad).BlockTableRecordHandle;var block=ModelBlock(cad);
  foreach(var entity in entities) {entity.SoftOwner=owner;if(string.IsNullOrEmpty(entity.ObjectHandle))entity.ObjectHandle=Handle();block.AddEntity(entity);}
  cad.Entities=cad.Entities.Concat(entities).ToArray();
 }
 static void SaveCad(CadImage cad,string path) {cad.Save(path,new DxfOptions {OutputMode=CadOutputMode.Convert});}











 static CadRasterImage AddRaster(CadImage cad,string path,double x=600) {
  Require(File.Exists(path),"Raster source is missing: "+path);string owner=Model(cad).BlockTableRecordHandle;
  CadRasterImageDef definition;using(Image pixels=Image.Load(path))definition=new CadRasterImageDef(Path.GetFullPath(path),pixels.Width,pixels.Height) {ObjectHandle=Handle(),SoftOwner=owner};
  cad.Objects=cad.Objects.Concat(new CadBaseObject[]{definition}).ToArray();
  var image=new CadRasterImage(definition,new Cad3DPoint(x,0,0),new Cad3DPoint(1,0,0),new Cad3DPoint(0,1,0)) {ObjectHandle=Handle(),SoftOwner=owner,ImageDefReference=definition.ObjectHandle};Add(cad,image);return image;
 }




 static int Main(string[] args) {try {
string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");

 if(!Input("background.png"))return 0;
 var expected=new Dictionary<string,Cad2DPoint>();
 using(var cad=(CadImage)Image.Load(input)) {
  var blocks=cad.BlockEntities.Values.Cast<CadBlockEntity>().ToArray();Console.WriteLine("Accessible block entities: "+blocks.Sum(b=>b.Entities.Count));
  var hatches=cad.Entities.Concat(blocks.SelectMany(b=>b.Entities)).OfType<CadHatch>().GroupBy(h=>h.ObjectHandle).Select(g=>g.First()).Where(h=>h.PatternDefinitions!=null&&h.PatternDefinitions.Count>0).ToArray();Require(hatches.Length>0,"No patterned hatch origin to adjust.");
  AddRaster(cad,"background.png");
  foreach(var h in hatches) {foreach(var line in h.PatternDefinitions)line.LineBasePoint=new Cad2DPoint(line.LineBasePoint.X+10,line.LineBasePoint.Y+15);expected[h.ObjectHandle]=h.PatternDefinitions[0].LineBasePoint;}
  SaveCad(cad,"output/origins.dxf");
 }
 using(var saved=(CadImage)Image.Load("output/origins.dxf")) {foreach(var kv in expected){var p=saved.Entities.OfType<CadHatch>().Single(h=>h.ObjectHandle==kv.Key).PatternDefinitions[0].LineBasePoint;Require(Math.Abs(p.X-kv.Value.X)<1e-8&&Math.Abs(p.Y-kv.Value.Y)<1e-8,"Pattern origin did not persist.");}}

Console.WriteLine("Verified task 45101");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
