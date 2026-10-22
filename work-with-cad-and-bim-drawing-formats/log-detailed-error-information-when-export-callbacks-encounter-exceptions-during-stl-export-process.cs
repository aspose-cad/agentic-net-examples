using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.stl";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            var loadOptions = new LoadOptions { IgnoreErrors = true };
            CadImage cadImage = (CadImage)Image.Load(inputPath, loadOptions);

            // STL export is not supported by Aspose.CAD in the current SDK.
            // Consequently, we cannot attach a render callback or perform the export.
            // If future SDK versions introduce StlOptions, the export logic and
            // detailed error logging using CadRenderResult would be implemented here.

            Console.WriteLine("STL export is not available in the current Aspose.CAD SDK.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}