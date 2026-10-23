using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Dwf;
using Aspose.CAD.FileFormats.Dwf.Whip.Objects.Drawable.Text;
using Aspose.CAD.FileFormats.Dwf.Whip.Objects;

namespace CadAdjustDwfWhipText
{
    class Program
    {
        static void Main()
        {
            const string inputPath = "input.dwf";
            const string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the file as a generic Image
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to DwfImage for DWF-specific operations
                    DwfImage dwfImage = image as DwfImage;
                    if (dwfImage == null)
                    {
                        Console.Error.WriteLine("The input file is not a DWF image that supports DwfWhipText manipulation.");
                        return;
                    }

                    bool anyTextModified = false;
                    const double offsetX = 10.0;
                    const double offsetY = 5.0;

                    // Iterate through pages and adjust DwfWhipText positions
                    foreach (var page in dwfImage.Pages)
                    {
                        var textEntities = page.Entities.OfType<DwfWhipText>();
                        foreach (var textEntity in textEntities)
                        {
                            if (textEntity.Position != null)
                            {
                                textEntity.Position.X += offsetX;
                                textEntity.Position.Y += offsetY;
                                anyTextModified = true;
                            }
                        }
                    }

                    if (!anyTextModified)
                    {
                        Console.WriteLine("No DwfWhipText entities were found to modify.");
                    }
                    else
                    {
                        Console.WriteLine("DwfWhipText positions have been adjusted.");
                    }

                    // Ensure output directory exists
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Prepare PDF export options
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800f,
                        PageHeight = 600f,
                        BackgroundColor = Aspose.CAD.Color.White,
                        UnitType = UnitType.Millimeter,
                        Zoom = 1.0f
                    };

                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the modified DWF as PDF
                    dwfImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"Modified CAD saved to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}