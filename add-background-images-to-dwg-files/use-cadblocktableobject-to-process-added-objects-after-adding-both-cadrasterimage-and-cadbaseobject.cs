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
 static string Handle() {return "F"+Guid.NewGuid().ToString("N").Substring(0,14).ToUpperInvariant();}
 static CadLayout Model(CadImage cad) {return cad.Layouts.Values.First(l=>l.LayoutName.Equals("Model",StringComparison.OrdinalIgnoreCase));}
 static CadBlockEntity ModelBlock(CadImage cad) {string owner=Model(cad).BlockTableRecordHandle;return cad.BlockEntities.Values.Cast<CadBlockEntity>().First(b=>b.CadBlockTableObject?.ObjectHandle==owner);}
 static void Add(CadImage cad,params CadEntityBase[] entities) {
  string owner=Model(cad).BlockTableRecordHandle;var block=ModelBlock(cad);
  foreach(var entity in entities) {entity.SoftOwner=owner;if(string.IsNullOrEmpty(entity.ObjectHandle))entity.ObjectHandle=Handle();block.AddEntity(entity);}
  cad.Entities=cad.Entities.Concat(entities).ToArray();
 }
 static void SaveCad(CadImage cad,string path) {cad.Save(path,new DxfOptions {OutputMode=CadOutputMode.Convert});}




 static CadRasterImage AddRaster(CadImage cad,string rasterPath,double x=0,double y=0) {
  Require(File.Exists(rasterPath),"Raster source is missing: "+rasterPath);
  string owner=Model(cad).BlockTableRecordHandle;
  CadRasterImageDef definition;
  using(Image pixels=Image.Load(rasterPath)) definition=new CadRasterImageDef(rasterPath,pixels.Width,pixels.Height) {ObjectHandle=Handle(),SoftOwner=owner};
  // CadRasterImageDef is the CadBaseObject stored in Objects; image instances are entities.
  CadBaseObject definitionObject=definition;cad.Objects=cad.Objects.Concat(new[]{definitionObject}).ToArray();
  var image=new CadRasterImage(definition,new Cad3DPoint(x,y,0),new Cad3DPoint(1,0,0),new Cad3DPoint(0,1,0)) {ObjectHandle=Handle(),SoftOwner=owner,ImageDefReference=definition.ObjectHandle};
  Add(cad,image);return image;
 }

 static int Main(string[] args) {try {

 string input="input.dwg",rasterPath="background.png";if(!Input(input)||!Input(rasterPath))return 0;Directory.CreateDirectory("output");
 string handle,definitionHandle,owner;int before;
 using(var cad=(CadImage)Image.Load(input)) {
  before=cad.Entities.Count();owner=Model(cad).BlockTableRecordHandle;
  cad.UpdateSize(true);double x=cad.MaxPoint.X+100;
  var rasterEntity=AddRaster(cad,rasterPath,x,0);handle=rasterEntity.ObjectHandle;definitionHandle=rasterEntity.ImageDefReference;
  var record=ModelBlock(cad).CadBlockTableObject;
  Require(cad.BlocksTables.Cast<CadBlockTableObject>().Any(r=>r.ObjectHandle==record.ObjectHandle),"Model block record is unregistered.");
  Require(ModelBlock(cad).Entities.Any(e=>e.ObjectHandle==handle),"Raster missing from the model block.");
  cad.UpdateSize(true);cad.GetBounds(rasterEntity);
  Require(rasterEntity.Bounds!=null&&rasterEntity.Bounds.Count>0,"Referenced raster bounds were not calculated.");
  Require(cad.MaxPoint.X>=x,"Drawing size does not include the external raster reference.");
  Console.WriteLine("Model layout including resolved raster reference: "+cad.MinPoint.X+","+cad.MinPoint.Y+" to "+cad.MaxPoint.X+","+cad.MaxPoint.Y);
  SaveCad(cad,"output/background.dxf");
 }
 using(var saved=(CadImage)Image.Load("output/background.dxf")) {
  var rasterEntity=saved.Entities.OfType<CadRasterImage>().Single(r=>r.ObjectHandle==handle);
  Require(saved.Entities.Count()==before+1&&rasterEntity.SoftOwner==owner&&rasterEntity.ImageDefReference==definitionHandle,"Raster ownership/reference did not persist.");
  var definition=saved.Objects.OfType<CadRasterImageDef>().Single(d=>d.ObjectHandle==definitionHandle);
  Require(!string.IsNullOrWhiteSpace(definition.FileName)&&File.Exists(definition.FileName),"Saved raster path is unresolved.");
 }

 Console.WriteLine("Verified task 45050");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
