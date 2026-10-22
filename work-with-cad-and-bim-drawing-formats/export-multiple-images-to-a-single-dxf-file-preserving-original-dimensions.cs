using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            // Expect two arguments: input CAD file path and output directory.
            if (args.Length != 2)
            {
                Console.Error.WriteLine("Usage: CadProcessor <input-cad-file> <output-directory>");
                return;
            }

            string inputPath = args[0];
            string outputDir = args[1];

            // Validate input file.
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                return;
            }

            // Ensure output directory exists.
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            try
            {
                // Load the CAD file.
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage for object‑model access.
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Loaded image is not a CAD image.");
                        return;
                    }

                    // List all layers in the drawing.
                    Console.WriteLine("Layers in the drawing:");
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        Console.WriteLine($"- {layer.Name} (On: {!(layer.ColorId < 0)})");
                    }

                    // Example: filter entities belonging to the first layout (if any).
                    string[] targetLayouts = cadImage.Layouts.Keys.Take(1).ToArray();
                    if (targetLayouts.Length == 0)
                    {
                        Console.WriteLine("No layouts found; using all entities.");
                    }

                    // Prepare rasterization options common to raster exports.
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter,
                        LineScale = 0.5f,
                        PageWidth = 210f,   // A4 width in mm (converted by UnitType)
                        PageHeight = 297f,  // A4 height in mm
                        AutomaticLayoutsScaling = true,
                        Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 },
                        Layouts = targetLayouts
                    };

                    // Export to PDF.
                    string pdfPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".pdf");
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };
                    cadImage.Save(pdfPath, pdfOptions);
                    Console.WriteLine($"Saved PDF: {pdfPath}");

                    // Export to SVG.
                    string svgPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".svg");
                    SvgOptions svgOptions = new SvgOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };
                    cadImage.Save(svgPath, svgOptions);
                    Console.WriteLine($"Saved SVG: {svgPath}");

                    // Export to PNG.
                    string pngPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".png");
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };
                    cadImage.Save(pngPath, pngOptions);
                    Console.WriteLine($"Saved PNG: {pngPath}");

                    // Export to DXF after possible modifications.
                    // Example modification: add a red line to the first layout.
                    CadLine newLine = new CadLine
                    {
                        FirstPoint = new Cad3DPoint(0, 0, 0),
                        SecondPoint = new Cad3DPoint(100, 0, 0),
                        LayerName = "0",
                        ColorId = 1 // Red (ACI 1)
                    };
                    // Insert the new line into the block entities collection.
                    CadBlockEntity modelSpace = cadImage.BlockEntities.ValuesTyped.FirstOrDefault(b => b.Name == "*Model_Space");
                    if (modelSpace != null)
                    {
                        modelSpace.AddEntity(newLine);
                    }

                    string dxfPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + "_modified.dxf");
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,
                        Version = DxfOutputVersion.R12,
                        DxfFileFormat = CadFileFormat.Binary
                    };
                    cadImage.Save(dxfPath, dxfOptions);
                    Console.WriteLine($"Saved modified DXF: {dxfPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}