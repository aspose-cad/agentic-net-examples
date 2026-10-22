using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace CadProcessingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Validate arguments
                if (args.Length == 0)
                {
                    Console.Error.WriteLine("Usage: CadProcessingExample <input-cad-file>");
                    return;
                }

                string inputPath = args[0];
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                    return;
                }

                // Prepare output directory
                string outputDir = Path.Combine(Path.GetDirectoryName(inputPath) ?? "", "output");
                Directory.CreateDirectory(outputDir);

                // Load CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage for CAD-specific operations
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // List layers and their visibility status
                    Console.WriteLine("Layers in the CAD file:");
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        string layerName = layer.Name;
                        bool isOff = layer.ColorId < 0;
                        bool isFrozen = (layer.Flags & 1) != 0;
                        bool isLocked = (layer.Flags & 0x10) != 0; // bit 4
                        Console.WriteLine($"- {layerName} (Off: {isOff}, Frozen: {isFrozen}, Locked: {isLocked})");
                    }

                    // Determine a layout to render (use first layout if available)
                    string layoutToRender = cadImage.Layouts.Keys.FirstOrDefault();

                    // Common rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Render the selected layout; if none, default layout is used
                        Layouts = string.IsNullOrEmpty(layoutToRender) ? null : new string[] { layoutToRender },
                        // Set high quality for text, hatch and arcs
                        Quality = new RasterizationQuality
                        {
                            Text = RasterizationQualityValue.High,
                            Hatch = RasterizationQualityValue.High,
                            Arc = RasterizationQualityValue.High
                        }
                    };

                    // Export to PNG
                    string pngPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".png");
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };
                    cadImage.Save(pngPath, pngOptions);
                    Console.WriteLine($"PNG saved to: {pngPath}");

                    // Export to PDF
                    string pdfPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".pdf");
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };
                    cadImage.Save(pdfPath, pdfOptions);
                    Console.WriteLine($"PDF saved to: {pdfPath}");

                    // Export to SVG
                    string svgPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".svg");
                    SvgOptions svgOptions = new SvgOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };
                    cadImage.Save(svgPath, svgOptions);
                    Console.WriteLine($"SVG saved to: {svgPath}");

                    // Save as DXF (convert if original format differs)
                    string dxfPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".dxf");
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,
                        DxfFileFormat = CadFileFormat.Binary,
                        Version = DxfOutputVersion.R12
                    };
                    cadImage.Save(dxfPath, dxfOptions);
                    Console.WriteLine($"DXF saved to: {dxfPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}