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
string input="input.dwg";if(!Input(input))return 0;Directory.CreateDirectory("output");

 string raster=args.Length>0?args[0]:"background.png";if(!File.Exists(raster))throw new FileNotFoundException("Background image path is missing or invalid.",raster);
 using(var cad=(CadImage)Image.Load(input)) {var image=AddRaster(cad,raster);Require(cad.Objects.OfType<CadRasterImageDef>().Any(d=>d.ObjectHandle==image.ImageDefReference),"Raster definition missing.");SaveCad(cad,"output/background.dxf");}

Console.WriteLine("Verified task 45058");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
