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

    static bool Input(string path, bool explicitInput) {if(File.Exists(path))return true;Console.Error.WriteLine("Input file not found: "+path);return false;}

    static void Pdf(Image image,string output,CadRasterizationOptions raster) {bool complete=false;int calls=0,failures=0;raster.RenderResult=r=>{calls++;complete=r.IsRenderComplete;failures+=r.Failures.Count;};image.Save(output,new PdfOptions{VectorRasterizationOptions=raster});Require(calls>0&&complete&&failures==0&&File.Exists(output)&&new FileInfo(output).Length>100,"PDF export incomplete or failed.");}

    static int Main(string[] args)
    {
        try
        {
            string input=args.Length>0?args[0]:"input.dwg";
            if(!Input(input,args.Length>0))return args.Length==0?0:1;
            Directory.CreateDirectory("output");
            using(var cad=(CadImage)Image.Load(input)) {
                var before=Snapshot(cad);
                var raster=new CadRasterizationOptions {PageWidth=800,PageHeight=600,
                    Quality=new RasterizationQuality {Text=RasterizationQualityValue.High,Hatch=RasterizationQualityValue.High,Arc=RasterizationQualityValue.High},
                    GraphicsOptions=new GraphicsOptions {SmoothingMode=Aspose.CAD.SmoothingMode.AntiAlias,TextRenderingHint=Aspose.CAD.TextRenderingHint.AntiAliasGridFit}};
                Pdf(cad,"output/high-quality.pdf",raster);
                var after=Snapshot(cad);Require(before.Count==after.Count&&before.All(k=>after.TryGetValue(k.Key,out var value)&&value==k.Value),"Export changed native geometry.");
            }

            Console.WriteLine("Verified task 45578");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;}
    }
}
