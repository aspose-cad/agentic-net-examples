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
 static bool Input(string path) {if(File.Exists(path))return true;Console.WriteLine("Input file not found: "+path);return false;}
 static string Handle() {return "F"+Guid.NewGuid().ToString("N").Substring(0,14).ToUpperInvariant();}

 static void SaveCad(CadImage cad,string path) {cad.Save(path,new DxfOptions {OutputMode=CadOutputMode.Convert});}

 static void AddTo(CadImage cad,CadLayout layout,CadEntityBase entity) {
  string owner=layout.BlockTableRecordHandle;var block=cad.BlockEntities.Values.Cast<CadBlockEntity>().Single(b=>b.CadBlockTableObject?.ObjectHandle==owner);
  entity.SoftOwner=owner;if(string.IsNullOrEmpty(entity.ObjectHandle))entity.ObjectHandle=Handle();block.AddEntity(entity);cad.Entities=cad.Entities.Concat(new[]{entity}).ToArray();
 }
 static CadLwPolyline Rectangle() {return new CadLwPolyline {Flag=CadLwPolylineFlag.Closed,PointCount=4,Coordinates=new List<Cad2DPoint>{new Cad2DPoint(10,10),new Cad2DPoint(110,10),new Cad2DPoint(110,60),new Cad2DPoint(10,60)},LayerName="0",ColorId=3,ConstantWidth=1.25};}
 static IEnumerable<CadLwPolyline> Polylines(CadImage cad) {return cad.Entities.Concat(cad.BlockEntities.Values.Cast<CadBlockEntity>().SelectMany(b=>b.Entities)).OfType<CadLwPolyline>().GroupBy(p=>p.ObjectHandle).Select(g=>g.First());}
 static string Geometry(CadLwPolyline p) {return p.Flag+"|"+p.Elevation+"|"+string.Join(";",p.Coordinates.Select(v=>v.X+","+v.Y))+"|"+string.Join(",",Enumerable.Range(0,p.Coordinates.Count).Select(i=>p.Bugle!=null&&i<p.Bugle.Count?p.Bugle[i]:0));}

 static int Main(string[] args) {try {
string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");

 string layoutName=args.Length>0?args[0]:"Model";string handle,owner,shape;string[] original;
 using(var cad=(CadImage)Image.Load(input)) {
  CadLayout layout=cad.Layouts.Values.SingleOrDefault(l=>l.LayoutName.Equals(layoutName,StringComparison.OrdinalIgnoreCase));Require(layout!=null,"Requested layout does not exist: "+layoutName);owner=layout.BlockTableRecordHandle;original=cad.Entities.Select(e=>e.ObjectHandle).ToArray();
  var polyline=Rectangle();AddTo(cad,layout,polyline);handle=polyline.ObjectHandle;shape=Geometry(polyline);SaveCad(cad,"output/polyline.dxf");
 }
 using(var saved=(CadImage)Image.Load("output/polyline.dxf")) {
  var polyline=Polylines(saved).Single(p=>p.ObjectHandle==handle);Require(polyline.SoftOwner==owner&&polyline.LayerName=="0"&&polyline.ColorId==3&&polyline.ConstantWidth==1.25&&polyline.PointCount==4&&Geometry(polyline)==shape,"Polyline geometry, layer/color, width or layout ownership did not persist.");
  Require(original.All(saved.Entities.Select(e=>e.ObjectHandle).Contains),"Existing entities were lost.");Console.WriteLine("Verified layer 0 / color 3 in layout "+layoutName);
 }

 Console.WriteLine("Verified task 45007");return 0;
} catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;} }
}
