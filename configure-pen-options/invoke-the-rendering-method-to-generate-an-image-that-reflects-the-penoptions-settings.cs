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
                    // Define input CAD file path (adjust as needed)
                    string inputFilePath = "sample.dwg";

                    // Verify that the input file exists
                    if (!File.Exists(inputFilePath))
                    {
                        Console.WriteLine($"Input file '{inputFilePath}' not found. Please provide a valid CAD file.");
                        return;
                    }

                    // Create output directory
                    string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
                    Directory.CreateDirectory(outputDir);

                    // Load CAD image and cast to CadImage for object model access
                    using (Image cadImage = Image.Load(inputFilePath))
                    {
                        CadImage cad = cadImage as CadImage;
                        if (cad == null)
                        {
                            Console.WriteLine("Failed to cast loaded image to CadImage.");
                            return;
                        }

                        // Inspect block definitions
                        Console.WriteLine("Block Definitions:");
                        foreach (CadBlockEntity block in cad.BlockEntities.ValuesTyped)
                        {
                            Console.WriteLine($"Block Name: {block.Name}");
                            foreach (CadEntityBase entity in block.Entities)
                            {
                                string typeName = entity.GetType().Name;
                                Console.WriteLine($"  Entity Type: {typeName}");
                                // Example of handling specific entity types
                                if (entity is CadLine line)
                                {
                                    Console.WriteLine($"    Line from ({line.FirstPoint.X}, {line.FirstPoint.Y}) to ({line.SecondPoint.X}, {line.SecondPoint.Y})");
                                }
                                else if (entity is CadLwPolyline polyline)
                                {
                                    Console.WriteLine($"    Polyline with {polyline.Coordinates.Count} points");
                                }
                                else if (entity is CadInsertObject insert)
                                {
                                    Console.WriteLine($"    Insert of block '{insert.Name}' with scales X:{insert.ScaleX} Y:{insert.ScaleY} Z:{insert.ScaleZ}");
                                }
                            }
                        }

                        // Export the CAD drawing to PDF
                        string pdfOutputPath = Path.Combine(outputDir, "output.pdf");
                        PdfOptions pdfOptions = new PdfOptions();
                        cad.Save(pdfOutputPath, pdfOptions);
                        Console.WriteLine($"PDF saved to: {pdfOutputPath}");

                        // Export the CAD drawing to SVG
                        string svgOutputPath = Path.Combine(outputDir, "output.svg");
                        SvgOptions svgOptions = new SvgOptions();
                        cad.Save(svgOutputPath, svgOptions);
                        Console.WriteLine($"SVG saved to: {svgOutputPath}");

                        // Export the CAD drawing to PNG with rasterization options
                        string pngOutputPath = Path.Combine(outputDir, "output.png");
                        PngOptions pngOptions = new PngOptions();
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1200,
                            Layouts = new string[] { "Model" } // Adjust layout name as needed
                        };
                        pngOptions.VectorRasterizationOptions = rasterOptions;
                        cad.Save(pngOutputPath, pngOptions);
                        Console.WriteLine($"PNG saved to: {pngOutputPath}");
                    }
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}