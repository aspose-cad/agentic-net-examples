using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.ImageOptions;

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
    static Cad3DPoint CopyPoint(Cad3DPoint p) => p == null ? null : new Cad3DPoint(p.X,p.Y,p.Z);
    static string Geometry(CadEntityBase entity)
    {
        if (entity is CadSolid solid) return "solid:"+Point(solid.FirstCorner)+":"+Point(solid.SecondCorner)+":"+Point(solid.ThirdCorner)+":"+Point(solid.FourthCorner)+":"+Number(solid.Thickness);
        if (entity is CadArc arc) return "arc:"+Point(arc.CenterPoint)+":"+Number(arc.Radius)+":"+Number(arc.StartAngle)+":"+Number(arc.EndAngle)+":"+Point(arc.ExtrusionDirection)+":"+Number(arc.Thickness);
        if (entity is CadCircle circle) return "circle:"+Point(circle.CenterPoint)+":"+Number(circle.Radius)+":"+Point(circle.ExtrusionDirection)+":"+Number(circle.Thickness);
        if (entity is CadLine line) return "line:"+Point(line.FirstPoint)+":"+Point(line.SecondPoint)+":"+Point(line.ExtrusionDirection)+":"+Number(line.Thickness);
        if (entity is CadText text) return "text:"+text.DefaultValue+":"+Point(text.FirstAlignment)+":"+Point(text.SecondAlignmentPoint)+":"+Number(text.TextHeight)+":"+Number(text.TextRotation);
        if (entity is CadMText mtext) return "mtext:"+mtext.Text+":"+Point(mtext.InsertionPoint)+":"+Number(mtext.InitialTextHeight)+":"+Number(mtext.RotationAngleRad);
        if (entity is CadLwPolyline poly) return "lwpoly:"+poly.Flag+":"+Number(poly.Elevation)+":"+string.Join(";",poly.Coordinates.Select(p=>Number(p.X)+","+Number(p.Y)))+":"+string.Join(",",(poly.Bugle??new List<double>()).Select(Number));
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
    static string[] Blocks(CadImage image) => image.BlockEntities.Values.Cast<CadBlockEntity>().Select(b=>b.Name+"|"+b.CadBlockTableObject.ObjectHandle+"|"+b.SoftOwner+"|"+b.Entities.Count).OrderBy(x=>x).ToArray();

    static CadEntityBase ImportEntity(CadEntityBase source, CadImage donor, CadImage target)
    {
        CadEntityBase copy;
        if (source is CadLine line) copy=new CadLine(CopyPoint(line.FirstPoint),CopyPoint(line.SecondPoint)) {Thickness=line.Thickness, ExtrusionDirection=CopyPoint(line.ExtrusionDirection)};
        else if (source is CadCircle circle && source is not CadArc) copy=new CadCircle { CenterPoint=CopyPoint(circle.CenterPoint), Radius=circle.Radius, Thickness=circle.Thickness, ExtrusionDirection=CopyPoint(circle.ExtrusionDirection) };
        else if (source is CadText text)
        {
            // Text fonts and styles must already match; do not silently change appearance.
            var donorStyle=donor.Styles.Cast<Aspose.CAD.FileFormats.Cad.CadTables.CadStyleTableObject>().Single(s=>s.StyleName==text.StyleType);
            var targetStyle=target.Styles.Cast<Aspose.CAD.FileFormats.Cad.CadTables.CadStyleTableObject>().SingleOrDefault(s=>s.StyleName==text.StyleType);
            Require(targetStyle!=null && targetStyle.PrimaryFontName==donorStyle.PrimaryFontName && targetStyle.BigFontName==donorStyle.BigFontName, "External text style requires dependency import.");
            copy=new CadText { DefaultValue=text.DefaultValue, FirstAlignment=CopyPoint(text.FirstAlignment), SecondAlignmentPoint=CopyPoint(text.SecondAlignmentPoint), TextHeight=text.TextHeight, TextRotation=text.TextRotation, ScaleX=text.ScaleX, GenerationFlag=text.GenerationFlag, HorizontalJustification=text.HorizontalJustification, VerticalJustification=text.VerticalJustification, Thickness=text.Thickness, Elevation=text.Elevation, ExtrusionDirection=CopyPoint(text.ExtrusionDirection), ObliqueAngle=text.ObliqueAngle, StyleType=text.StyleType };
        }
        else throw new NotSupportedException("The external DXF importer supports LINE, CIRCLE and TEXT only; "+source.TypeName+" requires dependency-aware import.");
        Require((source.ChildObjects==null || source.ChildObjects.Count==0) && (source.XdataContainer?.Xdatas==null || !source.XdataContainer.Xdatas.Any()), "External extended data or child entities require dependency-aware import.");
        copy.LayerName=source.LayerName;copy.ColorId=source.ColorId;copy.ColorValue=source.ColorValue;copy.LineWeight=source.LineWeight;copy.LineScale=source.LineScale;copy.LineTypeName=source.LineTypeName;
        return copy;
    }

    static int Main(string[] args)
    {
        string temporary="output/inserted.pending.dwg";
        try
        {
            string input=args.Length>0?args[0]:"input.dwg", external=args.Length>1?args[1]:"external.dxf";
            if (!File.Exists(input) || !File.Exists(external)) { Console.WriteLine("Input file not found: "+(!File.Exists(input)?input:external)); return 0; }
            Directory.CreateDirectory("output");
            string output="output/inserted.dwg";
            string name="IMPORTED_"+Guid.NewGuid().ToString("N").Substring(0,12).ToUpperInvariant();
            using (var source=(CadImage)Image.Load(input))
            using (var donor=(CadImage)Image.Load(external))
            {
                var original=Snapshot(source);var layouts=Layouts(source);var blocks=Blocks(source);int before=source.Entities.Count();
                string donorOwner=donor.Layouts.Values.Single(l=>l.LayoutName.Equals("Model",StringComparison.OrdinalIgnoreCase)).BlockTableRecordHandle;
                var externalEntities=donor.Entities.Where(e=>e.SoftOwner==donorOwner).ToArray();
                Require(externalEntities.Length>0,"External DXF model is empty.");
                Require(externalEntities.All(e=>source.Layers.GetLayersNames().Contains(e.LayerName)),"External layers require dependency import.");
                Require(externalEntities.All(e=>string.IsNullOrEmpty(e.LineTypeName) || e.LineTypeName.Equals("BYLAYER",StringComparison.OrdinalIgnoreCase) || e.LineTypeName.Equals("BYBLOCK",StringComparison.OrdinalIgnoreCase) || e.LineTypeName.Equals("CONTINUOUS",StringComparison.OrdinalIgnoreCase)),"Custom external linetypes require dependency import.");
                var imported=externalEntities.Select(e=>ImportEntity(e,donor,source)).ToArray();
                var geometry=imported.Select(Geometry).ToArray();
                using (var writer=NativeWriter(source))
                {
                    writer.AddBlock(name,"0");
                    foreach (var entity in imported) writer.AddEntityToBlock(name,entity);
                    writer.AddEntity(new CadInsertObject { Name=name, InsertionPoint=new Cad3DPoint(25,30,0), ScaleX=1, ScaleY=1, ScaleZ=1, LayerName="0" });
                    writer.Save(temporary,new DwgOptions { OutputMode=CadOutputMode.Convert });
                }
                using (var saved=(CadImage)Image.Load(temporary))
                {
                    var after=Snapshot(saved);
                    Require(original.All(pair=>after.TryGetValue(pair.Key,out var value)&&value==pair.Value),"Existing geometry, handles or ownership changed.");
                    Require(Layouts(saved).SequenceEqual(layouts),"Existing layout ownership changed.");
                    Require(Blocks(saved).Where(b=>!b.StartsWith(name+"|",StringComparison.OrdinalIgnoreCase)&&!b.StartsWith("*Model_Space|",StringComparison.OrdinalIgnoreCase)).SequenceEqual(blocks.Where(b=>!b.StartsWith("*Model_Space|",StringComparison.OrdinalIgnoreCase))),"Existing block definitions changed.");
                    Require(saved.Entities.Count()==before+1 && saved.BlockEntities.ContainsKey(name),"Imported insert or block did not persist.");
                    var insert=saved.Entities.OfType<CadInsertObject>().Single(i=>i.Name==name);
                    Require(Point(insert.InsertionPoint)=="25,30,0" && insert.ScaleX==1 && insert.ScaleY==1 && insert.ScaleZ==1,"Insert placement changed.");
                    var block=saved.BlockEntities[name];Require(block.Entities.Count==geometry.Length && block.Entities.Select(Geometry).SequenceEqual(geometry),"Imported geometry changed.");
                    Require(block.Entities.All(e=>e.SoftOwner==block.CadBlockTableObject.ObjectHandle),"Imported entity ownership is incorrect.");
                    Console.WriteLine("Preserved "+original.Count+" existing entities, "+layouts.Length+" layouts; imported "+geometry.Length+" entities through "+insert.TypeName+".");
                }
            }
            File.Move(temporary,output,true);
            Console.WriteLine("Verified task 45757");return 0;
        }
        catch(Exception ex) { if(File.Exists(temporary)) File.Delete(temporary); Console.Error.WriteLine(ex.Message);return 1; }
    }
}
