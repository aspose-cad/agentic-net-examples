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
 static CadRasterizationOptions Raster() {return new CadRasterizationOptions {PageWidth=800,PageHeight=600,BackgroundColor=Color.White};}
 static void Pdf(Image image,string path,CadRasterizationOptions raster) {
  bool called=false,complete=false;int errors=0;
  raster.RenderResult=result=>{called=true;complete=result.IsRenderComplete;errors+=result.Failures.Count;foreach(var failure in result.Failures)Console.WriteLine("Render diagnostic: "+failure.RenderCode+" "+failure.Message);};
  image.Save(path,new PdfOptions {VectorRasterizationOptions=raster});
  Require(called&&complete&&errors==0,"PDF render callback did not confirm a complete render.");
  Require(File.ReadAllBytes(path).Take(5).SequenceEqual(System.Text.Encoding.ASCII.GetBytes("%PDF-")),"PDF header missing.");
 }


 static int Main(string[] args) {try {

 string input="input.dwg";if(!Input(input))return 0;Directory.CreateDirectory("output");
 string textHandle=Handle(),mtextHandle=Handle(),owner;int before;
 using(var cad=(CadImage)Image.Load(input)) {
  owner=Model(cad).BlockTableRecordHandle;before=cad.Entities.Count();
  var existing=cad.Entities.OfType<CadText>().ToArray();
  // Place the new annotations beyond the rightmost measured existing text bound.
  cad.GetBounds();
  double right=existing.SelectMany(t=>t.Bounds??new List<Cad3DPoint>()).Select(p=>p.X).DefaultIfEmpty(cad.MaxPoint.X).Max();
  double x=right+20,y=existing.Select(t=>t.FirstAlignment.Y).DefaultIfEmpty(0).Max();
  var text=new CadText {ObjectHandle=textHandle,SoftOwner=owner,DefaultValue="New annotation",TextHeight=5,FirstAlignment=new Cad3DPoint(x,y,0)};
  var mtext=new CadMText {ObjectHandle=mtextHandle,SoftOwner=owner,Text="Multiline annotation\\PSecond line",InitialTextHeight=5,ReferenceRectangleWidth=100,InsertionPoint=new Cad3DPoint(x,y-20,0)};
  // Register both entities in the actual model block and drawing collection.
  Add(cad,text,mtext);Require(text.FirstAlignment.X>right&&mtext.InsertionPoint.X>right,"Annotations overlap existing text bounds.");
  Require(ModelBlock(cad).Entities.Any(e=>e.ObjectHandle==textHandle)&&ModelBlock(cad).Entities.Any(e=>e.ObjectHandle==mtextHandle),"Annotations were not registered in the model block before Save.");
  SaveCad(cad,"output/annotations.dxf");
 }
 using(var saved=(CadImage)Image.Load("output/annotations.dxf")) {
  var text=saved.Entities.OfType<CadText>().Single(t=>t.ObjectHandle==textHandle);
  var mtext=saved.Entities.OfType<CadMText>().Single(t=>t.ObjectHandle==mtextHandle);
  Require(saved.Entities.Count()==before+2 && text.SoftOwner==owner && mtext.SoftOwner==owner,"Entities or model ownership did not persist.");
  Require(text.DefaultValue=="New annotation"&&mtext.Text.Contains("Second line"),"Annotation content did not persist.");
  // On DXF reload model content is enumerated through drawing.Entities and SoftOwner.
  Require(ModelBlock(saved).CadBlockTableObject.ObjectHandle==text.SoftOwner,"Saved text does not belong to the model block record.");
  Pdf(saved,"output/annotations.pdf",Raster());
 }

 Console.WriteLine("Verified task 44749");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
