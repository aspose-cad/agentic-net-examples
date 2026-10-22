using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.UnderlayDefinition;
using Aspose.CAD.ImageOptions;

namespace CadExternalReferenceVerifier
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the input CAD file
                string inputPath = "sample.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD image
                CadImage cad = (CadImage)Image.Load(inputPath);

                // Iterate through all CAD objects to find external references
                foreach (var baseObj in cad.Objects)
                {
                    if (baseObj is CadRasterImageDef rasterDef)
                    {
                        string rasterFile = rasterDef.FileName;
                        bool rasterExists = !string.IsNullOrEmpty(rasterFile) && File.Exists(rasterFile);
                        Console.WriteLine($"Raster Image Definition (Handle: {rasterDef.ObjectHandle}) - File: \"{rasterFile}\" Exists: {rasterExists}");
                    }
                    else if (baseObj is CadUnderlayDefinition underlayDef)
                    {
                        string underlayFile = underlayDef.UnderlayPathAndFileName;
                        bool underlayExists = !string.IsNullOrEmpty(underlayFile) && File.Exists(underlayFile);
                        Console.WriteLine($"Underlay Definition (Handle: {underlayDef.ObjectHandle}) - File: \"{underlayFile}\" Exists: {underlayExists}");
                    }
                }

                // Save the CAD file without modifications (optional)
                string outputPath = "output.dwg";
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = Aspose.CAD.FileFormats.Cad.CadConsts.CadFileFormat.Binary
                };
                cad.Save(outputPath, saveOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}