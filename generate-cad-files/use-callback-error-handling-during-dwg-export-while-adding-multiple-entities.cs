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

    static void SaveWithCallback(DwgImage drawing,string path,Action<Exception> onFailure) {try{drawing.Save(path,new DwgOptions{OutputMode=CadOutputMode.Convert});}catch(Exception ex){onFailure(ex);throw;}}

    static int Main(string[] args)
    {
        try
        {
            Directory.CreateDirectory("output");string path=args.Length>0?args[0]:"output/callback.dwg";int failures=0;
            using(var dwg=new DwgImage(CadAcadVersion.AC1032)) {
                dwg.AddEntity(new CadLine(new Cad3DPoint(10,20,0),new Cad3DPoint(110,80,0)));
                dwg.AddEntity(new CadCircle{CenterPoint=new Cad3DPoint(50,50,0),Radius=15});
                // Native Convert Save has no SDK render callback. This application callback handles exceptions.
                Action<Exception> onExportFailure=ex=>{failures++;Console.Error.WriteLine("Application export failure callback: "+ex.Message);};
                SaveWithCallback(dwg,path,onExportFailure);Require(failures==0,"The failure callback reported an export error.");
            }
            Require(File.Exists(path)&&new FileInfo(path).Length>6,"DWG output is missing or empty.");
            using(var saved=(CadImage)Image.Load(path)){Require(saved.Entities.OfType<CadLine>().Count()==1&&saved.Entities.OfType<CadCircle>().Count()==1,"Added entities did not persist.");var line=saved.Entities.OfType<CadLine>().Single();var circle=saved.Entities.OfType<CadCircle>().Single();Require(Point(line.FirstPoint)=="10,20,0"&&Point(line.SecondPoint)=="110,80,0"&&Point(circle.CenterPoint)=="50,50,0"&&circle.Radius==15,"Added geometry changed.");}

            Console.WriteLine("Verified task 44705");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;}
    }
}
