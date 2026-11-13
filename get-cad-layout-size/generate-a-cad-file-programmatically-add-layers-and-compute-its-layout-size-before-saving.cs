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

    static CadLayout Model(CadImage image) => image.Layouts.Values.Single(l=>l.LayoutName.Equals("Model",StringComparison.OrdinalIgnoreCase));

    static int Main(string[] args)
    {
        try
        {
            int requested=args.Length>0?int.Parse(args[0],CultureInfo.InvariantCulture):3;Require(requested>=1&&requested<=255,"Layer color must be an indexed color from 1 to 255.");short color=checked((short)requested);
            Directory.CreateDirectory("output");double width,height;
            using(var dwg=new DwgImage(CadAcadVersion.AC1032)) {
                var layer=dwg.AddLayer("MyLayer");layer.ColorId=color;layer.Flags=0;layer.PlotFlag=true;
                dwg.AddEntity(new CadLine(new Cad3DPoint(10,20,0),new Cad3DPoint(110,80,0)){LayerName="MyLayer"});dwg.UpdateSize(true);width=dwg.MaxPoint.X-dwg.MinPoint.X;height=dwg.MaxPoint.Y-dwg.MinPoint.Y;
                Require(Math.Abs(width-100)<1e-8&&Math.Abs(height-60)<1e-8,"Measured model geometry does not match the created line.");dwg.Save("output/layers.dwg",new DwgOptions{OutputMode=CadOutputMode.Convert});
            }
            Require(File.Exists("output/layers.dwg")&&new FileInfo("output/layers.dwg").Length>6,"DWG output is missing or empty.");
            using(var saved=(CadImage)Image.Load("output/layers.dwg")){Require(saved.Layers.Cast<CadLayerTable>().Single(l=>l.Name=="MyLayer").ColorId==color,"Layer color was not saved.");var line=saved.Entities.OfType<CadLine>().Single();Require(line.LayerName=="MyLayer"&&line.SoftOwner==Model(saved).BlockTableRecordHandle&&Point(line.FirstPoint)=="10,20,0"&&Point(line.SecondPoint)=="110,80,0","Layer, model ownership or geometry changed.");}
            Console.WriteLine("Measured model layout: "+width+" x "+height);

            Console.WriteLine("Verified task 45730");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;}
    }
}
