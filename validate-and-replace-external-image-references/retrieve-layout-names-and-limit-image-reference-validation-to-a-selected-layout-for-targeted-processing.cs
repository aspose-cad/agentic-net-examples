using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.UnderlayDefinition;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (CadImage cad = (CadImage)Image.Load(inputPath))
            {
                // -------------------------------------------------
                // 1. Retrieve and display all layout names
                // -------------------------------------------------
                Console.WriteLine("Available Layouts:");
                foreach (var layoutKvp in cad.Layouts)
                {
                    var layout = layoutKvp.Value;
                    Console.WriteLine($"- {layout.LayoutName}");
                }

                // -------------------------------------------------
                // 2. Select a target layout (first one)
                // -------------------------------------------------
                var targetLayout = cad.Layouts.Values.FirstOrDefault();
                if (targetLayout == null)
                {
                    Console.WriteLine("No layouts found in the drawing.");
                    return;
                }
                Console.WriteLine($"\nProcessing layout: {targetLayout.LayoutName}");

                // NOTE: The Aspose.CAD API does not expose a direct property linking
                // an entity to a layout. Therefore the validation below processes
                // all entities in the drawing. If layout association becomes available,
                // the filter can be added here.

                // -------------------------------------------------
                // 3. Validate raster image references
                // -------------------------------------------------
                var rasterDefs = cad.Objects
                                   .OfType<CadRasterImageDef>()
                                   .ToDictionary(def => def.ObjectHandle, def => def);

                foreach (var entity in cad.Entities)
                {
                    if (entity.TypeName != CadEntityTypeName.IMAGE)
                        continue;

                    var rasterImage = entity as CadRasterImage;
                    if (rasterImage == null)
                        continue;

                    if (rasterDefs.TryGetValue(rasterImage.ImageDefReference, out CadRasterImageDef def))
                    {
                        if (!File.Exists(def.FileName))
                        {
                            Console.WriteLine($"Missing raster image file: {def.FileName}");
                        }
                        else
                        {
                            var ip = rasterImage.InsertionPoint;
                            Console.WriteLine($"Raster image '{def.FileName}' is valid (Insertion Point: {ip.X}, {ip.Y}, {ip.Z})");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Raster definition with handle '{rasterImage.ImageDefReference}' not found.");
                    }
                }

                // -------------------------------------------------
                // 4. Validate underlay definitions
                // -------------------------------------------------
                var underlayDefs = cad.Objects
                                      .OfType<CadUnderlayDefinition>()
                                      .ToDictionary(def => def.ObjectHandle, def => def);

                foreach (var entity in cad.Entities)
                {
                    if (entity.TypeName != CadEntityTypeName.UNDERLAY)
                        continue;

                    var underlayEntity = entity as CadUnderlay;
                    if (underlayEntity == null)
                        continue;

                    string defHandle = underlayEntity.AcDbUnderlayDefinitionObjectId;

                    if (underlayDefs.TryGetValue(defHandle, out CadUnderlayDefinition udef))
                    {
                        if (!File.Exists(udef.UnderlayPathAndFileName))
                        {
                            Console.WriteLine($"Missing underlay file: {udef.UnderlayPathAndFileName}");
                        }
                        else
                        {
                            Console.WriteLine($"Underlay '{udef.UnderlayPathAndFileName}' is valid.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Underlay definition with handle '{defHandle}' not found.");
                    }
                }

                // -------------------------------------------------
                // 5. Save the CAD file (preserving editable objects)
                // -------------------------------------------------
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? ".");

                var saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary,
                    Version = DxfOutputVersion.R12
                };

                cad.Save(outputPath, saveOptions);
                Console.WriteLine($"File saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}