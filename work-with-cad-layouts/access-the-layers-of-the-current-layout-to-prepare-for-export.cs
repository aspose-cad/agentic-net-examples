using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Source file not found: {inputPath}");
                return;
            }

            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load CAD file with error ignoring
            LoadOptions loadOptions = new LoadOptions { IgnoreErrors = true };
            using (Image image = Image.Load(inputPath, loadOptions))
            {
                CadImage cadImage = (CadImage)image;

                // List layers
                Console.WriteLine("Layers:");
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool isFrozen = (layer.Flags & 1) != 0;
                    bool isLocked = (layer.Flags & 4) != 0;
                    bool isOff = layer.ColorId < 0;
                    Console.WriteLine($"  Name: {layer.Name}, Frozen: {isFrozen}, Locked: {isLocked}, Off: {isOff}");
                }

                // List block definitions
                Console.WriteLine("Block Definitions:");
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"  Block Name: {block.Name}, Entity Count: {block.Entities.Count}");
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        Console.WriteLine($"    Entity Type: {entity.GetType().Name}, Layer: {entity.LayerName}");
                    }
                }

                // Export to PDF (vector rasterization)
                CadRasterizationOptions pdfRasterOptions = new CadRasterizationOptions
                {
                    // Example: render the Model layout
                    Layouts = new string[] { "Model" },
                    // Render all layers
                    Layers = null,
                    // High quality settings
                    Quality = new RasterizationQuality
                    {
                        Text = RasterizationQualityValue.High,
                        Hatch = RasterizationQualityValue.High,
                        Arc = RasterizationQualityValue.High
                    },
                    // Optional render result callback
                    RenderResult = result =>
                    {
                        if (!result.IsRenderComplete)
                        {
                            Console.Error.WriteLine("PDF render incomplete.");
                        }
                        if (result.Failures != null && result.Failures.Count > 0)
                        {
                            foreach (var failure in result.Failures)
                            {
                                Console.Error.WriteLine($"PDF render failure: {failure}");
                            }
                        }
                    }
                };

                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = pdfRasterOptions
                };

                string pdfPath = Path.Combine(outputDir, "output.pdf");
                cadImage.Save(pdfPath, pdfOptions);
                Console.WriteLine($"PDF saved to {pdfPath}");

                // Export to PNG with a specific layer (e.g., layer named "0")
                CadRasterizationOptions pngRasterOptions = new CadRasterizationOptions
                {
                    Layouts = new string[] { "Model" },
                    Layers = new string[] { "0" },
                    // Adjust line thickness if needed
                    LineScale = 1.0f,
                    Quality = new RasterizationQuality
                    {
                        Text = RasterizationQualityValue.High,
                        Hatch = RasterizationQualityValue.High,
                        Arc = RasterizationQualityValue.High
                    },
                    RenderResult = result =>
                    {
                        if (!result.IsRenderComplete)
                        {
                            Console.Error.WriteLine("PNG render incomplete.");
                        }
                        if (result.Failures != null && result.Failures.Count > 0)
                        {
                            foreach (var failure in result.Failures)
                            {
                                Console.Error.WriteLine($"PNG render failure: {failure}");
                            }
                        }
                    }
                };

                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = pngRasterOptions
                };

                string pngPath = Path.Combine(outputDir, "output.png");
                cadImage.Save(pngPath, pngOptions);
                Console.WriteLine($"PNG saved to {pngPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}