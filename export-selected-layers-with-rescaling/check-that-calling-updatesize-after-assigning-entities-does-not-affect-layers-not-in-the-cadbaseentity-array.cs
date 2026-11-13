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

    static bool Input(string path, bool explicitInput) {if(File.Exists(path))return true;Console.Error.WriteLine("Input file not found: "+path);return false;}

    static int Main(string[] args)
    {
        try
        {
            string input=args.Length>0?args[0]:"input.dxf";
            if(!Input(input,args.Length>0))return args.Length==0?0:1;
            Directory.CreateDirectory("output");
            string layer=args.Length>1?args[1]:"0";
            using(var cad=(CadImage)Image.Load(input)) {
                var all=cad.Entities.ToArray();var selected=all.Where(e=>e.LayerName==layer).ToArray();var outside=all.Where(e=>e.LayerName!=layer).ToArray();
                Require(selected.Length>0&&outside.Length>0,"The check requires selected and unselected layer entities.");
                var original=outside.ToDictionary(e=>e.ObjectHandle,Signature);var layers=cad.Layers.Cast<CadLayerTable>().Select(l=>l.Name+"|"+l.ColorId+"|"+l.Flags).ToArray();
                try {cad.Entities=selected;cad.UpdateSize(true);Require(outside.All(e=>original[e.ObjectHandle]==Signature(e)),"Unselected geometry or layer assignments changed.");Require(layers.SequenceEqual(cad.Layers.Cast<CadLayerTable>().Select(l=>l.Name+"|"+l.ColorId+"|"+l.Flags)),"Layer definitions changed.");}
                finally {cad.Entities=all;cad.UpdateSize(true);}
                Require(cad.Entities.Count()==all.Length,"Original drawing collection was not restored.");Console.WriteLine("Preserved "+outside.Length+" unselected entities and "+layers.Length+" layer definitions.");
            }

            Console.WriteLine("Verified task 45559");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;}
    }
}
