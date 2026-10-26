using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;

class Program {
 static void Require(bool condition, string message) { if(!condition) throw new InvalidOperationException(message); }
 static string Handle() { return "F"+Guid.NewGuid().ToString("N").Substring(0,14).ToUpperInvariant(); }
 static int Main(string[] args) {
  try {

   string inputPath=args.Length>0?args[0]:"input.dwg";string rasterPath="background.png";
   if(!File.Exists(inputPath)||!File.Exists(rasterPath)) {Console.WriteLine("Input file not found.");return 0;}
   Directory.CreateDirectory("output");
   string defHandle,owner;int count,definitions;string first=Handle(),second=Handle();
   using(var cad=(CadImage)Image.Load(inputPath)) {
    owner=cad.Layouts.Values.First(l=>l.LayoutName.Equals("Model",StringComparison.OrdinalIgnoreCase)).BlockTableRecordHandle;
    count=cad.Entities.Count();definitions=cad.Objects.OfType<CadRasterImageDef>().Count();
    var shared=cad.Objects.OfType<CadRasterImageDef>().FirstOrDefault(d=>string.Equals(d.FileName,rasterPath,StringComparison.OrdinalIgnoreCase));
    Console.WriteLine("Reused existing raster definition: "+(shared!=null));
    if(shared==null) {
     using(Image pixels=Image.Load(rasterPath)) shared=new CadRasterImageDef(rasterPath,pixels.Width,pixels.Height) {ObjectHandle=Handle(),SoftOwner=owner};
     // Set a nonempty unique definition handle BEFORE constructing either image.
     cad.Objects=cad.Objects.Concat(new CadBaseObject[]{shared}).ToArray();definitions++;
    }
    Require(!string.IsNullOrWhiteSpace(shared.ObjectHandle),"Existing raster definition has no handle.");
    defHandle=shared.ObjectHandle;
    var a=new CadRasterImage(shared,new Cad3DPoint(0,0,0),new Cad3DPoint(1,0,0),new Cad3DPoint(0,1,0)) {ObjectHandle=first,SoftOwner=owner,ImageDefReference=defHandle};
    var b=new CadRasterImage(shared,new Cad3DPoint(100,100,0),new Cad3DPoint(.5,0,0),new Cad3DPoint(0,.5,0)) {ObjectHandle=second,SoftOwner=owner,ImageDefReference=defHandle};
    cad.Entities=cad.Entities.Concat(new CadEntityBase[]{a,b}).ToArray();
    var model=cad.BlockEntities.Values.Cast<CadBlockEntity>().First(block=>block.CadBlockTableObject?.ObjectHandle==owner);
    model.AddEntity(a);model.AddEntity(b);
    cad.UpdateSize(true);
    cad.Save("output/reused-raster.dxf",new DxfOptions {OutputMode=CadOutputMode.Convert});
   }
   using(var saved=(CadImage)Image.Load("output/reused-raster.dxf")) {
    Console.WriteLine("Raster roundtrip counts: before="+count+", after="+saved.Entities.Count()+", expected pair="+first+","+second+", owner="+owner);
    foreach(var item in saved.Entities.OfType<CadRasterImage>()) Console.WriteLine("Raster "+item.ObjectHandle+" def="+item.ImageDefReference+" owner="+item.SoftOwner);
    var pair=saved.Entities.OfType<CadRasterImage>().Where(r=>r.ObjectHandle==first||r.ObjectHandle==second).ToArray();
    Require(saved.Entities.Count()==count+2 && pair.Length==2 && pair.All(r=>r.ImageDefReference==defHandle && r.SoftOwner==owner),"Raster instances or shared references did not persist.");
    Require(saved.Objects.OfType<CadRasterImageDef>().Count()==definitions && saved.Objects.OfType<CadRasterImageDef>().Count(d=>d.ObjectHandle==defHandle)==1,"Raster definition was duplicated.");
   }

   Console.WriteLine("Verified task 45067 with supplied drawing input.");

   return 0;
  } catch(Exception e) {Console.Error.WriteLine(e.Message);return 1;}
 }
}
