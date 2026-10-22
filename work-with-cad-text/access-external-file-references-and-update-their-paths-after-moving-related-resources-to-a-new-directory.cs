using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.UnderlayDefinition;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the original CAD file
            string sourceCadPath = @"C:\CAD\original.dxf";

            // Directory where external raster and underlay files have been moved
            string newResourcesDir = @"C:\CAD\Resources";

            // Validate inputs
            if (!File.Exists(sourceCadPath))
            {
                Console.Error.WriteLine("Source CAD file not found.");
                return;
            }

            if (!Directory.Exists(newResourcesDir))
            {
                Console.Error.WriteLine("Resources directory not found.");
                return;
            }

            // Load the CAD image
            CadImage cadImage = (CadImage)Image.Load(sourceCadPath);

            // Update external raster image definitions
            foreach (var obj in cadImage.Objects)
            {
                if (obj is CadRasterImageDef rasterDef)
                {
                    string fileNameOnly = Path.GetFileName(rasterDef.FileName);
                    string newFilePath = Path.Combine(newResourcesDir, fileNameOnly);
                    rasterDef.FileName = newFilePath;
                }
            }

            // Update underlay definitions
            foreach (var obj in cadImage.Objects)
            {
                if (obj is CadUnderlayDefinition underlayDef)
                {
                    string fileNameOnly = Path.GetFileName(underlayDef.UnderlayPathAndFileName);
                    string newPath = Path.Combine(newResourcesDir, fileNameOnly);
                    underlayDef.UnderlayPathAndFileName = newPath;
                }
            }

            // Prepare output path and ensure directory exists
            string outputCadPath = @"C:\CAD\modified.dxf";
            string outputDir = Path.GetDirectoryName(outputCadPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the modified CAD file preserving editable objects
            DxfOptions dxfOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                DxfFileFormat = CadFileFormat.Ascii
            };
            cadImage.Save(outputCadPath, dxfOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}