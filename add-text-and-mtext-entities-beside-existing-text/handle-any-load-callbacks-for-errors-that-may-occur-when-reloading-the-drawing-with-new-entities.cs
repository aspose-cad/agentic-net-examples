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
















 static int Main(string[] args) {try {
string input="input.dxf";if(!Input(input))return 0;Directory.CreateDirectory("output");

 string[] original;string textHandle,mtextHandle,owner;
 using(var cad=(CadImage)Image.Load(input)) {
  original=cad.Entities.Select(e=>e.ObjectHandle).ToArray();owner=Model(cad).BlockTableRecordHandle;
  CadEntityBase text=new CadText {DefaultValue="New text",TextHeight=5,FirstAlignment=new Cad3DPoint(550,10,0)};
  CadEntityBase mtext=new CadMText {Text="New MTEXT",InitialTextHeight=5,ReferenceRectangleWidth=100,InsertionPoint=new Cad3DPoint(550,30,0)};
  Add(cad,text,mtext);textHandle=text.ObjectHandle;mtextHandle=mtext.ObjectHandle;SaveCad(cad,"output/text.dxf");
 }
 // LoadOptions has readable Errors diagnostics, not an assignable error callback.
 var load=new LoadOptions();
 using(var saved=(CadImage)Image.Load("output/text.dxf",load)) {
  foreach(var error in load.Errors)Console.Error.WriteLine("Load diagnostic: "+error);
  Require(load.Errors.Count==0,"Reload reported load errors.");
  Require(original.All(saved.Entities.Select(e=>e.ObjectHandle).Contains)&&saved.Entities.Count()==original.Length+2,"Existing/new entities were not preserved.");
  Require(saved.Entities.OfType<CadText>().Single(t=>t.ObjectHandle==textHandle).SoftOwner==owner&&saved.Entities.OfType<CadMText>().Single(t=>t.ObjectHandle==mtextHandle).Text=="New MTEXT","Text or model ownership did not persist.");
 }

Console.WriteLine("Verified task 45135");return 0;
} catch(Exception ex) {Console.Error.WriteLine(ex.Message);return 1;} }
}
