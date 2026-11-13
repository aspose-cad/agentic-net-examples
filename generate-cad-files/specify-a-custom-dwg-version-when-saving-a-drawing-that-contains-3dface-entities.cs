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

    static int Main(string[] args)
    {
        try
        {
            string revision=args.Length>0?args[0]:"AC1032";
            Require(Enum.TryParse<CadAcadVersion>(revision,out var version)&&Enum.IsDefined(typeof(CadAcadVersion),version)&&version!=CadAcadVersion.NONE,"Unknown DWG revision: "+revision);
            Require(version==CadAcadVersion.AC1032,"Unsupported DWG revision in the verified writer: "+revision+". Only AC1032 is verified with this SDK; no version downgrade is performed.");
            Directory.CreateDirectory("output");string path="output/versioned.pending.dwg";
            using(var dwg=new DwgImage(version)) {
                dwg.AddBlock("FACE","0");var face=new Cad3DFace(new Cad3DPoint(0,0,0),new Cad3DPoint(100,0,0),new Cad3DPoint(100,80,30),new Cad3DPoint(0,80,30));dwg.AddEntityToBlock("FACE",face);dwg.AddEntity(new CadInsertObject{Name="FACE",InsertionPoint=new Cad3DPoint(0,0,0),ScaleX=1,ScaleY=1,ScaleZ=1});dwg.Save(path,new DwgOptions{OutputMode=CadOutputMode.Convert});
            }
            Require(File.Exists(path)&&new FileInfo(path).Length>6,"DWG output is missing or empty.");
            using(var stream=File.OpenRead(path)){byte[] header=new byte[6];Require(stream.Read(header)==6&&System.Text.Encoding.ASCII.GetString(header)==revision,"DWG writer did not preserve the requested revision.");}
            using(var saved=(CadImage)Image.Load(path)){var face=saved.BlockEntities.Values.Cast<CadBlockEntity>().SelectMany(b=>b.Entities).OfType<Cad3DFace>().Single();Require(Point(face.FirstCorner)=="0,0,0"&&Point(face.SecondCorner)=="100,0,0"&&Point(face.ThirdCorner)=="100,80,30"&&Point(face.FourthCorner)=="0,80,30","3DFACE corners did not persist.");Require(saved.Header.AcadVersion==version,"Reloaded DWG revision differs.");}

            File.Move(path,"output/versioned.dwg",true);
            Console.WriteLine("Verified task 44701");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;}
    }
}
