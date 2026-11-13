using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.FileFormats.Cad.CadObjects.UnderlayDefinition;
using Aspose.CAD.FileFormats.Dwf;

class Program
{
    static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    // Copy the public drawing state; a cast of Image.Load() to DwgImage is invalid.
    // Keep the source alive until the native builder has saved its drawing.
    static DwgImage NativeWriter(CadImage source)
    {
        return new DwgImage(CadAcadVersion.AC1032)
        {
            Header = source.Header,
            ApplicationVersion = source.ApplicationVersion,
            MaintenanceVersion = source.MaintenanceVersion,
            FileEncoding = source.FileEncoding,
            SpecifiedEncoding = source.SpecifiedEncoding,
            SpecifiedMifEncoding = source.SpecifiedMifEncoding,
            DefaultFont = source.DefaultFont,
            DefaultLineWeight = source.DefaultLineWeight,
            LineTypes = source.LineTypes,
            BlockEntities = source.BlockEntities,
            ClassEntities = source.ClassEntities,
            ThumbnailImage = source.ThumbnailImage,
            BlocksTables = source.BlocksTables,
            DimensionStyles = source.DimensionStyles,
            Entities = source.Entities,
            Objects = source.Objects,
            Layers = source.Layers,
            Styles = source.Styles,
            ViewPorts = source.ViewPorts,
            Views = source.Views,
            UCSs = source.UCSs,
            CadAcds = source.CadAcds,
            AppIdTables = source.AppIdTables,
            Palette = source.Palette
        };
    }

    static string Number(double n) => n.ToString("G12", CultureInfo.InvariantCulture);
    static string Point(Cad3DPoint p) => p == null ? "null" : Number(p.X)+","+Number(p.Y)+","+Number(p.Z);

    static string Geometry(CadEntityBase entity)
    {
        if (entity is CadSolid solid) return "solid:"+Point(solid.FirstCorner)+":"+Point(solid.SecondCorner)+":"+Point(solid.ThirdCorner)+":"+Point(solid.FourthCorner)+":"+Number(solid.Thickness);
        if (entity is CadArc arc) return "arc:"+Point(arc.CenterPoint)+":"+Number(arc.Radius)+":"+Number(arc.StartAngle)+":"+Number(arc.EndAngle)+":"+Point(arc.ExtrusionDirection)+":"+Number(arc.Thickness);
        if (entity is CadCircle circle) return "circle:"+Point(circle.CenterPoint)+":"+Number(circle.Radius)+":"+Point(circle.ExtrusionDirection)+":"+Number(circle.Thickness);
        if (entity is CadLine line) return "line:"+Point(line.FirstPoint)+":"+Point(line.SecondPoint)+":"+Point(line.ExtrusionDirection)+":"+Number(line.Thickness);
        if (entity is CadText text) return "text:"+text.DefaultValue+":"+Point(text.FirstAlignment)+":"+Point(text.SecondAlignmentPoint)+":"+Number(text.TextHeight)+":"+Number(text.TextRotation);
        if (entity is CadMText mtext) return "mtext:"+mtext.Text+":"+Point(mtext.InsertionPoint)+":"+Number(mtext.InitialTextHeight)+":"+Number(mtext.RotationAngleRad);
        if (entity is CadLwPolyline poly) return "lwpoly:"+poly.Flag+":"+Number(poly.Elevation)+":"+string.Join(";",poly.Coordinates.Select(p=>Number(p.X)+","+Number(p.Y)))+":"+string.Join(",",Enumerable.Range(0,poly.Coordinates.Count).Select(i=>Number(poly.Bugle!=null&&i<poly.Bugle.Count?poly.Bugle[i]:0)));
        if (entity is CadInsertObject insert) return "insert:"+insert.Name+":"+Point(insert.InsertionPoint)+":"+Number(insert.ScaleX)+":"+Number(insert.ScaleY)+":"+Number(insert.ScaleZ)+":"+Number(insert.RotationAngle);
        // Arbitrary geometry needs a type-specific comparison, not a count-only assertion.
        throw new NotSupportedException("This bounded example cannot verify preservation of "+entity.TypeName+".");
    }
    static string Signature(CadEntityBase entity) => entity.TypeName+"|"+entity.SoftOwner+"|"+entity.LayerName+"|"+entity.ColorId+"|"+Geometry(entity);
    static Dictionary<string,string> Snapshot(CadImage image)
    {
        var entities = image.Entities.Concat(image.BlockEntities.Values.Cast<CadBlockEntity>().SelectMany(b=>b.Entities)).Distinct();
        var result = new Dictionary<string,string>();
        foreach (var entity in entities)
        {
            Require(!string.IsNullOrEmpty(entity.ObjectHandle), "An existing entity has no stable handle.");
            Require(!result.ContainsKey(entity.ObjectHandle), "Duplicate existing entity handle: "+entity.ObjectHandle);
            result.Add(entity.ObjectHandle, Signature(entity));
        }
        return result;
    }
    static string[] Layouts(CadImage image) => image.Layouts.Values.Select(l=>l.LayoutName+"|"+l.ObjectHandle+"|"+l.BlockTableRecordHandle).OrderBy(x=>x).ToArray();

    static bool Input(string path, bool explicitInput) {if(File.Exists(path))return true;Console.Error.WriteLine("Input file not found: "+path);return false;}
    static CadLayout Model(CadImage image) => image.Layouts.Values.Single(l=>l.LayoutName.Equals("Model",StringComparison.OrdinalIgnoreCase));

    static int Main(string[] args)
    {
        try
        {
            string input=args.Length>0?args[0]:"input.dwg";
            if(!Input(input,args.Length>0))return args.Length==0?0:1;
            Directory.CreateDirectory("output");
            string name="BLOCK_"+Guid.NewGuid().ToString("N").Substring(0,10);Dictionary<string,string> before;string[] layouts;string handle;
            using(var source=(CadImage)Image.Load(input)) {
                before=Snapshot(source);layouts=Layouts(source);
                using(var dwg=NativeWriter(source)){dwg.AddBlock(name,"0");dwg.AddEntityToBlock(name,new CadLine(new Cad3DPoint(0,0,0),new Cad3DPoint(100,0,10)));var insert=new CadInsertObject{Name=name,InsertionPoint=new Cad3DPoint(10,20,0),ScaleX=2,ScaleY=2,ScaleZ=2,RotationAngle=0};dwg.AddEntity(insert);handle=insert.ObjectHandle;dwg.Save("output/insert.pending.dwg",new DwgOptions{OutputMode=CadOutputMode.Convert});}
            }
            using(var saved=(CadImage)Image.Load("output/insert.pending.dwg")){var after=Snapshot(saved);Require(before.All(k=>after.TryGetValue(k.Key,out var value)&&value==k.Value)&&layouts.SequenceEqual(Layouts(saved)),"Original drawing changed: "+string.Join(";",before.Where(k=>!after.TryGetValue(k.Key,out var value)||value!=k.Value).Take(3).Select(k=>k.Key+" OLD="+k.Value+" NEW="+(after.TryGetValue(k.Key,out var value)?value:"missing"))));var insert=saved.Entities.OfType<CadInsertObject>().Single(i=>i.Name==name);var block=saved.BlockEntities[name];Require(block!=null&&block.Entities.OfType<CadLine>().Count()==1,"Registered block did not persist.");Require(Point(insert.InsertionPoint)=="10,20,0"&&insert.ScaleX==2&&insert.ScaleY==2&&insert.ScaleZ==2&&insert.SoftOwner==Model(saved).BlockTableRecordHandle,"Insert placement, scale or ownership changed.");Console.WriteLine("Preserved "+before.Count+" existing objects and "+layouts.Length+" layouts; block and insert verified.");}

            File.Move("output/insert.pending.dwg","output/insert.dwg",true);
            Console.WriteLine("Verified task 45752");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;}
    }
}
