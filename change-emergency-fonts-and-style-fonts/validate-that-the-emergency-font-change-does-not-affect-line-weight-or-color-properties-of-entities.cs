using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace CadFontChange
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = @"input.dxf";
            string outputPath = @"output\output.dxf";

            try
            {
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Store original color information for each entity
                    var originalColors = cadImage.Entities
                        .ToDictionary(e => e.ObjectHandle,
                                      e => new { e.ColorId, e.ColorValue });

                    // NOTE: The CadEntityBase class does not expose a line weight property,
                    // so line weight validation cannot be performed with the current SDK.

                    // Change the emergency/default font
                    cadImage.DefaultFont = "Arial";

                    // Verify that color properties have not changed
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (originalColors.TryGetValue(entity.ObjectHandle, out var original))
                        {
                            if (entity.ColorId != original.ColorId ||
                                entity.ColorValue != original.ColorValue)
                            {
                                Console.WriteLine($"Entity '{entity.ObjectHandle}' color changed after font update.");
                            }
                        }
                    }

                    // Ensure output directory exists
                    string outDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outDir))
                    {
                        Directory.CreateDirectory(outDir);
                    }

                    // Save as DXF with explicit binary format
                    var dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,
                        DxfFileFormat = CadFileFormat.Binary
                    };

                    cadImage.Save(outputPath, dxfOptions);
                }

                Console.WriteLine("Processing completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}