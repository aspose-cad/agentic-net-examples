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
        // Paths for input and output CAD files
        string inputPath = @"C:\CAD\input.dxf";
        string outputPath = @"C:\CAD\output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string baseFolder = Path.GetDirectoryName(inputPath);
        if (string.IsNullOrEmpty(baseFolder))
        {
            Console.WriteLine("Unable to determine base folder for input file.");
            return;
        }

        CadImage cadImage = null;
        try
        {
            cadImage = Image.Load(inputPath) as CadImage;
            if (cadImage == null)
            {
                Console.WriteLine("Failed to load CAD image.");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading CAD file: {ex.Message}");
            return;
        }

        // ---- Update external raster image definitions (IMAGEDEF) ----
        try
        {
            foreach (var obj in cadImage.Objects)
            {
                if (obj is CadRasterImageDef rasterDef)
                {
                    if (!string.IsNullOrEmpty(rasterDef.FileName) && Path.IsPathRooted(rasterDef.FileName))
                    {
                        string relativePath = Path.GetRelativePath(baseFolder, rasterDef.FileName);
                        rasterDef.FileName = relativePath;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing raster image definitions: {ex.Message}");
            return;
        }

        // ---- Update underlay definitions (e.g., PDF, DWF) ----
        try
        {
            foreach (var obj in cadImage.Objects)
            {
                if (obj is CadUnderlayDefinition underlayDef)
                {
                    if (!string.IsNullOrEmpty(underlayDef.UnderlayPathAndFileName) && Path.IsPathRooted(underlayDef.UnderlayPathAndFileName))
                    {
                        string relativePath = Path.GetRelativePath(baseFolder, underlayDef.UnderlayPathAndFileName);
                        underlayDef.UnderlayPathAndFileName = relativePath;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing underlay definitions: {ex.Message}");
            return;
        }

        // Ensure output directory exists
        try
        {
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating output directory: {ex.Message}");
            return;
        }

        // Save the modified CAD drawing preserving editable objects
        try
        {
            var saveOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                DxfFileFormat = CadFileFormat.Binary
            };
            cadImage.Save(outputPath);
            Console.WriteLine($"File saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving CAD file: {ex.Message}");
        }
    }
}