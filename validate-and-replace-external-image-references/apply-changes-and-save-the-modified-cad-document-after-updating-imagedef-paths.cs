using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.UnderlayDefinition;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dxf";
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        var outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the existing CAD file
            var cadImage = (CadImage)Image.Load(inputPath);

            // Update external raster definitions (IMAGEDEF) and underlay definitions
            foreach (var obj in cadImage.Objects)
            {
                if (obj is CadRasterImageDef rasterDef)
                {
                    // Set the new file name (full path) for the raster image
                    rasterDef.FileName = @"C:\Images\NewImage.png";

                    // Ensure every raster image entity points to the updated definition
                    foreach (var entity in cadImage.Entities)
                    {
                        if (entity is CadRasterImage rasterImg)
                        {
                            rasterImg.ImageDefReference = rasterDef.ObjectHandle;
                        }
                    }
                }
                else if (obj is CadUnderlayDefinition underlayDef)
                {
                    // Set the new path for the underlay file
                    underlayDef.UnderlayPathAndFileName = @"C:\Underlays\NewUnderlay.dwf";
                }
            }

            // Save the modified CAD document preserving editable objects
            var saveOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                DxfFileFormat = CadFileFormat.Binary
            };
            cadImage.Save(outputPath, saveOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}