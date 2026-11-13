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

    static bool Input(string path, bool explicitInput) {if(File.Exists(path))return true;Console.Error.WriteLine("Input file not found: "+path);return false;}

    static int Main(string[] args)
    {
        try
        {
            string input=args.Length>0?args[0]:"input.dwfx";if(!Input(input,args.Length>0))return args.Length==0?0:1;Directory.CreateDirectory("output");
            using(Image image=Image.Load(input)){Require(image is DwfImage,"DWFx input is required.");bool complete=false;int failures=0;var raster=new CadRasterizationOptions{PageWidth=800,PageHeight=600,RenderResult=r=>{complete=r.IsRenderComplete;failures+=r.Failures.Count;}};image.Save("output/rendered.png",new PngOptions{VectorRasterizationOptions=raster});Require(complete&&failures==0,"DWFx rendering failed.");}
            using(Image png=Image.Load("output/rendered.png")){Require(png.Width==800&&png.Height==600,"Exported PNG dimensions differ.");Console.WriteLine("Rendered PNG dimensions: "+png.Width+"x"+png.Height);}

            Console.WriteLine("Verified task 44154");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;}
    }
}
