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
            string input=args.Length>0?args[0]:"input.dwf";if(!Input(input,args.Length>0))return args.Length==0?0:1;
            using(Image image=Image.Load(input)){Require(image is DwfImage,"The input is not a DWF/DWFx model.");var dwf=(DwfImage)image;Console.WriteLine("Loaded DWF: "+image.Width+" x "+image.Height+"; pages "+dwf.Pages.Length);Require(dwf.Pages.Length>0,"DWF has no pages.");}

            Console.WriteLine("Verified task 44123");return 0;
        }
        catch(Exception ex){Console.Error.WriteLine(ex.Message);return 1;}
    }
}
