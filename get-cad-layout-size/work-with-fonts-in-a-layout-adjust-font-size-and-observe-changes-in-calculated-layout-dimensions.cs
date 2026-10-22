using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AsposeCadFontExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path (replace with actual file)
                string inputPath = "input.dwg";
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Retrieve the first layout (if any)
                    CadLayout targetLayout = null;
                    foreach (var layout in cadImage.Layouts.Values)
                    {
                        targetLayout = layout;
                        break;
                    }

                    if (targetLayout == null)
                    {
                        Console.Error.WriteLine("No layout found in the CAD file.");
                        return;
                    }

                    // Adjust font size for all text entities in the image
                    foreach (var entity in cadImage.Entities)
                    {
                        if (entity is CadText textEntity)
                        {
                            textEntity.TextHeight = textEntity.TextHeight * 1.5;
                        }
                    }

                    // Output layout dimensions after font size change
                    double width = targetLayout.MaxExtents.X - targetLayout.MinExtents.X;
                    double height = targetLayout.MaxExtents.Y - targetLayout.MinExtents.Y;
                    Console.WriteLine($"Layout \"{targetLayout.LayoutName}\" dimensions after font size change: Width = {width}, Height = {height}");

                    // Prepare rasterization options for the specific layout
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = new[] { targetLayout.LayoutName },
                        UnitType = UnitType.Centimenter,
                        LineScale = 1.0f,
                        PageWidth = 800,
                        PageHeight = 600,
                        AutomaticLayoutsScaling = true,
                        Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
                    };

                    // Save the result to PDF to observe the changes
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    string outputPath = "output.pdf";
                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"Saved output PDF to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}