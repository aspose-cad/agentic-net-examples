using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace RemoveCanvasMargins
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = args.Length > 0 ? args[0] : "sample.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // List layers and their visibility status
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        bool isOff = layer.ColorId < 0;
                        bool isFrozen = (layer.Flags & 1) != 0;
                        bool isLocked = (layer.Flags & 16) != 0;
                        Console.WriteLine($"Layer: {layer.Name}, Off: {isOff}, Frozen: {isFrozen}, Locked: {isLocked}");
                    }

                    // List block definitions and entity counts
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block: {block.Name}, EntityCount: {block.Entities.Count}");
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            if (entity is CadArc arc)
                            {
                                Console.WriteLine($"  Arc Center: ({arc.CenterPoint.X}, {arc.CenterPoint.Y}), StartAngle: {arc.StartAngle}, EndAngle: {arc.EndAngle}");
                            }
                        }
                    }

                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Zoom = 1,
                        Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
                    };

                    // Set up PNG export options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    string outputPath = Path.Combine(outputDir, "output.png");
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Export completed: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}