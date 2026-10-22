using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Validate input argument
                if (args == null || args.Length == 0 || string.IsNullOrEmpty(args[0]))
                {
                    Console.Error.WriteLine("Error: Input CAD file path must be provided as the first argument.");
                    return;
                }

                string inputPath = args[0];
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Error: Input file \"{inputPath}\" does not exist.");
                    return;
                }

                // Prepare output directory
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage for block and entity access
                    CadImage cad = cadImage as CadImage;
                    if (cad == null)
                    {
                        Console.Error.WriteLine("Error: Loaded image is not a CAD image.");
                        return;
                    }

                    // ---------- Export to PDF ----------
                    string pdfPath = Path.Combine(outputDir, "Result.pdf");
                    PdfOptions pdfOptions = new PdfOptions();
                    cad.Save(pdfPath, pdfOptions);
                    Console.WriteLine($"PDF saved to: {pdfPath}");

                    // ---------- Export to SVG ----------
                    string svgPath = Path.Combine(outputDir, "Result.svg");
                    SvgOptions svgOptions = new SvgOptions();
                    cad.Save(svgPath, svgOptions);
                    Console.WriteLine($"SVG saved to: {svgPath}");

                    // ---------- Export to PNG (raster) ----------
                    string pngPath = Path.Combine(outputDir, "Result.png");
                    PngOptions pngOptions = new PngOptions();
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Example raster settings; adjust as needed
                        PageWidth = 1600,
                        PageHeight = 1200,
                        Layouts = new string[] { "Model" } // Export the Model layout
                    };
                    pngOptions.VectorRasterizationOptions = rasterOptions;
                    cad.Save(pngPath, pngOptions);
                    Console.WriteLine($"PNG saved to: {pngPath}");

                    // ---------- Inspect Block Definitions ----------
                    Console.WriteLine("\nBlock Definitions:");
                    foreach (CadBlockEntity block in cad.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block Name: {block.Name}, Entity Count: {block.Entities.Count}");
                    }

                    // ---------- Inspect Top‑Level Entities ----------
                    Console.WriteLine("\nTop‑Level Entities:");
                    foreach (CadEntityBase entity in cad.Entities)
                    {
                        // Handle line entities
                        if (entity is CadLine line)
                        {
                            Console.WriteLine($"Line: Start({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) " +
                                              $"End({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z}) " +
                                              $"Layer: {line.LayerName}");
                        }
                        // Handle arc entities (must be before circle)
                        else if (entity is CadArc arc)
                        {
                            Console.WriteLine($"Arc: Center({arc.CenterPoint.X}, {arc.CenterPoint.Y}, {arc.CenterPoint.Z}) " +
                                              $"StartAngle: {arc.StartAngle}, EndAngle: {arc.EndAngle} " +
                                              $"Layer: {arc.LayerName}");
                        }
                        // Handle circle entities
                        else if (entity is CadCircle circle)
                        {
                            Console.WriteLine($"Circle: Center({circle.CenterPoint.X}, {circle.CenterPoint.Y}, {circle.CenterPoint.Z}) " +
                                              $"Radius: {circle.Radius} Layer: {circle.LayerName}");
                        }
                        // Handle lightweight polyline
                        else if (entity is CadLwPolyline polyline)
                        {
                            Console.WriteLine($"LwPolyline: VertexCount={polyline.Coordinates.Count} Layer: {polyline.LayerName}");
                        }
                        // Handle 3D face
                        else if (entity is Cad3DFace face)
                        {
                            Console.WriteLine($"3DFace: Corners=({face.FirstCorner.X},{face.FirstCorner.Y},{face.FirstCorner.Z}) " +
                                              $"({face.SecondCorner.X},{face.SecondCorner.Y},{face.SecondCorner.Z}) " +
                                              $"({face.ThirdCorner.X},{face.ThirdCorner.Y},{face.ThirdCorner.Z}) " +
                                              $"({face.FourthCorner.X},{face.FourthCorner.Y},{face.FourthCorner.Z}) " +
                                              $"Layer: {face.LayerName}");
                        }
                        // Handle block insert
                        else if (entity is CadInsertObject insert)
                        {
                            Console.WriteLine($"Insert: BlockName={insert.Name}, InsertionPoint=({insert.InsertionPoint.X}, {insert.InsertionPoint.Y}, {insert.InsertionPoint.Z}) " +
                                              $"ScaleX={insert.ScaleX}, ScaleY={insert.ScaleY}, ScaleZ={insert.ScaleZ}, Rotation={insert.RotationAngle} " +
                                              $"Layer: {insert.LayerName}");
                        }
                        else
                        {
                            // Other entity types can be added here
                            Console.WriteLine($"Entity of type {entity.GetType().Name} on layer {entity.LayerName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}