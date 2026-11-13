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
            string input=args.Length>0?args[0]:"input.dwf";if(!Input(input,args.Length>0))return args.Length==0?0:1;Directory.CreateDirectory("output");string[] layers;
            using(Image image=Image.Load(input)){Require(image is DwfImage,"The input is not DWF.");layers=((DwfImage)image).Layers.GetLayersNames().ToArray();foreach(string layer in layers)Console.WriteLine("Layer: "+layer);bool complete=false;int failures=0;var raster=new CadRasterizationOptions{PageWidth=800,PageHeight=600,RenderResult=r=>{complete=r.IsRenderComplete;failures+=r.Failures.Count;}};image.Save("output/drawing.jpg",new JpegOptions{Quality=90,VectorRasterizationOptions=raster});Require(complete&&failures==0,"DWF JPEG export failed.");}
            using(Image jpeg=Image.Load("output/drawing.jpg")){Require(jpeg.Width==800&&jpeg.Height==600,"Exported JPEG dimensions differ.");}long bytes=new FileInfo("output/drawing.jpg").Length;Require(bytes>0,"JPEG is empty.");Console.WriteLine("JPEG bytes: "+bytes+"; layers: "+layers.Length);

            Console.WriteLine("Verified task 44188");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;}
    }
}
