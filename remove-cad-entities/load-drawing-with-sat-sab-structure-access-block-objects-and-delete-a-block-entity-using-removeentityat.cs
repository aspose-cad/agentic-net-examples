using System;using System.IO;using System.Linq;using System.Collections.Generic;using System.Globalization;
using Aspose.CAD;using Aspose.CAD.ImageOptions;using Aspose.CAD.FileFormats.Cad;using Aspose.CAD.FileFormats.Cad.CadObjects;
class Program {
 static void Require(bool ok,string message){if(!ok)throw new InvalidOperationException(message);}
 static CadEntityBase[] All(CadImage cad)=>cad.Entities.Concat(cad.BlockEntities.Values.Cast<CadBlockEntity>().SelectMany(b=>b.Entities)).GroupBy(e=>e.ObjectHandle).Select(g=>g.First()).ToArray();
 static CadEntityBase[] Owned(CadImage cad,CadBlockEntity block)=>block.Entities.Concat(cad.Entities.Where(e=>e.SoftOwner==block.CadBlockTableObject.ObjectHandle)).GroupBy(e=>e.ObjectHandle).Select(g=>g.First()).ToArray();
 static string Point(Cad3DPoint point)=>point==null?"null":FormattableString.Invariant($"{point.X:G12},{point.Y:G12},{point.Z:G12}");
 static string Signature(CadEntityBase e) {
  string geometry;
  if(e is CadObjectWithAcisData acis)geometry=acis.ProprietaryDataParameterList==null?"null":string.Join(";",acis.ProprietaryDataParameterList.Select(p=>p.Key+"="+p.Value));
  else if(e is CadViewport view)geometry=Point(view.CenterPoint)+"|"+view.Width+"|"+view.Height+"|"+view.ViewHeight+"|"+view.ViewCenterPoint.X+","+view.ViewCenterPoint.Y+"|"+Point(view.ViewDirectionVector)+"|"+Point(view.ViewTargetVector);
  else if(e is CadLine line)geometry=Point(line.FirstPoint)+"|"+Point(line.SecondPoint);
  else if(e is CadCircle circle)geometry=Point(circle.CenterPoint)+"|"+circle.Radius;
  else throw new NotSupportedException("Preservation comparison is not implemented for "+e.TypeName);
  return e.TypeName+"|"+e.SoftOwner+"|"+e.LayerName+"|"+e.ColorId+"|"+geometry;
 }

 // Application helper: the SDK has no RemoveEntityAt method. Use its real removal API and collections.
 static CadEntityBase RemoveEntityAt(CadImage cad,CadBlockEntity block,int index){var values=Owned(cad,block);Require(index>=0&&index<values.Length,"Block entity index is out of range.");var entity=values[index];cad.TryRemoveEntity(entity);foreach(var b in cad.BlockEntities.Values.Cast<CadBlockEntity>())foreach(var match in b.Entities.Where(e=>e.ObjectHandle==entity.ObjectHandle).ToArray())b.Entities.Remove(match);cad.Entities=cad.Entities.Where(e=>e.ObjectHandle!=entity.ObjectHandle).ToArray();Require(!All(cad).Any(e=>e.ObjectHandle==entity.ObjectHandle),"Target was not removed from all owning collections.");return entity;}
 static int Main(string[] args){try{string input=args.Length>0?args[0]:"input.dxf";if(!File.Exists(input)){Console.Error.WriteLine("Input file not found: "+input);return args.Length==0?0:1;}Directory.CreateDirectory("output");Dictionary<string,string> before;string handle;string[] layouts;int index=args.Length>2?int.Parse(args[2],CultureInfo.InvariantCulture):0;using(Image loaded=Image.Load(input)){Require(loaded is CadImage,"A native DWG/DXF containing ACIS/SAT is required; standalone SAT/SAB has no native block collections.");var cad=(CadImage)loaded;Require(All(cad).OfType<CadObjectWithAcisData>().Any(a=>a.ProprietaryDataParameterList!=null&&a.ProprietaryDataParameterList.Any(p=>(p.Key==CadEntityAttribute.Cad001||p.Key==CadEntityAttribute.Cad003)&&!string.IsNullOrWhiteSpace(p.Value))),"No text ACIS/SAT data found.");var block=args.Length>1?cad.BlockEntities[args[1]]:cad.BlockEntities.Values.Cast<CadBlockEntity>().FirstOrDefault(b=>Owned(cad,b).Length>0);Require(block!=null,"Requested block is missing or empty.");before=All(cad).ToDictionary(e=>e.ObjectHandle,Signature);layouts=cad.Layouts.Values.Select(l=>l.LayoutName+"|"+l.BlockTableRecordHandle).OrderBy(x=>x).ToArray();handle=RemoveEntityAt(cad,block,index).ObjectHandle;cad.Save("output/removed.pending.dxf",new DxfOptions{OutputMode=CadOutputMode.Convert});}
 using(var saved=(CadImage)Image.Load("output/removed.pending.dxf")){var after=All(saved).ToDictionary(e=>e.ObjectHandle,Signature);Require(after.Count==before.Count-1&&!after.ContainsKey(handle),"Native reload did not preserve the exact deletion.");Require(before.Where(k=>k.Key!=handle).All(k=>after.TryGetValue(k.Key,out var value)&&value==k.Value),"A remaining entity or ACIS payload changed.");Require(layouts.SequenceEqual(saved.Layouts.Values.Select(l=>l.LayoutName+"|"+l.BlockTableRecordHandle).OrderBy(x=>x)),"Layout ownership changed.");Console.WriteLine("Removed one block entity; preserved "+after.Count+" remaining entity identities and ACIS payloads.");}
 File.Move("output/removed.pending.dxf","output/removed.dxf",true);
 Console.WriteLine("Verified task 45841");return 0;}catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;}}
}
