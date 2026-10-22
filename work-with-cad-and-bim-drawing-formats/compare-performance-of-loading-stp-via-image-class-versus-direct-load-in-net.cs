using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;

namespace CadProcessingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Expect two arguments: input CAD file path and output directory.
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: CadProcessingExample <input-cad-file> <output-directory>");
                return;
            }

            string inputPath = args[0];
            string outputDir = args[1];

            // Validate input file and output directory.
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                return;
            }

            if (!Directory.Exists(outputDir))
            {
                Console.Error.WriteLine($"Output directory does not exist: {outputDir}");
                return;
            }

            try
            {
                // Load the CAD file. Aspose.CAD.Image.Load returns an Image that can be cast to CadImage.
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // -----------------------------------------------------------------
                    // 1. List all layers present in the drawing.
                    // -----------------------------------------------------------------
                    Console.WriteLine("Layers in the drawing:");
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        Console.WriteLine($"  Layer Name: {layer.Name}, ColorId: {layer.ColorId}");
                    }

                    // -----------------------------------------------------------------
                    // 2. Inspect block definitions and their entities.
                    // -----------------------------------------------------------------
                    Console.WriteLine("\nBlock definitions and contained entities:");
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"  Block Name: {block.Name}, Entity Count: {block.Entities.Count}");
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            switch (entity)
                            {
                                case CadLine line:
                                    Console.WriteLine($"    Line: Start({line.FirstPoint.X},{line.FirstPoint.Y}) End({line.SecondPoint.X},{line.SecondPoint.Y})");
                                    break;
                                case CadLwPolyline polyline:
                                    Console.WriteLine($"    LWPolyline: Vertex Count = {polyline.Coordinates.Count}");
                                    break;
                                case CadArc arc:
                                    Console.WriteLine($"    Arc: Center({arc.CenterPoint.X},{arc.CenterPoint.Y}) Radius = {arc.Radius} StartAngle = {arc.StartAngle} EndAngle = {arc.EndAngle}");
                                    break;
                                case CadCircle circle:
                                    Console.WriteLine($"    Circle: Center({circle.CenterPoint.X},{circle.CenterPoint.Y}) Radius = {circle.Radius}");
                                    break;
                                case CadInsertObject insert:
                                    Console.WriteLine($"    Insert: Block Name = {insert.Name} Position({insert.InsertionPoint.X},{insert.InsertionPoint.Y}) Scale({insert.ScaleX},{insert.ScaleY},{insert.ScaleZ})");
                                    break;
                                case CadHatch hatch:
                                    Console.WriteLine($"    Hatch: Angle = {hatch.HatchAngle} ScaleOrSpacing = {hatch.HatchScaleOrSpacing} AssociativeFlag = {hatch.AssociativeFlag}");
                                    Console.WriteLine($"      Boundary Paths Count: {hatch.BoundaryPaths?.Count ?? 0}");
                                    break;
                                default:
                                    Console.WriteLine($"    Entity of type {entity.GetType().Name} encountered.");
                                    break;
                            }
                        }
                    }

                    // -----------------------------------------------------------------
                    // 3. Export the drawing to PDF (vector) and PNG (raster) with layout and layer filters.
                    // -----------------------------------------------------------------
                    // Define rasterization options common to both exports.
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Export only the "Model" layout if it exists; otherwise export all.
                        Layouts = new string[] { "Model" },
                        // Export only the layer named "0" if present.
                        Layers = new string[] { "0" },
                        // Set a reasonable image size for raster output.
                        PageWidth = 1200,
                        PageHeight = 800,
                        // Preserve line colors (object color) as line weight handling is not available.
                        DrawType = CadDrawTypeMode.UseObjectColor
                    };

                    // PDF export (vector).
                    string pdfPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".pdf");
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };
                    cadImage.Save(pdfPath, pdfOptions);
                    Console.WriteLine($"\nSaved PDF to: {pdfPath}");

                    // PNG export (raster).
                    string pngPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".png");
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };
                    cadImage.Save(pngPath, pngOptions);
                    Console.WriteLine($"Saved PNG to: {pngPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}