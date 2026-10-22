using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadTitleAdjustment
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "input.dwg";
                // Path for the exported PDF
                string outputPath = "output.pdf";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD drawing
                using (Image image = Image.Load(sourcePath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // Iterate over all CAD entities and adjust title text position
                    foreach (var entity in cadImage.Entities)
                    {
                        if (entity is CadText textEntity && !string.IsNullOrEmpty(textEntity.DefaultValue))
                        {
                            if (textEntity.DefaultValue.IndexOf("Title", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                // Move the title upward by 10 drawing units
                                Cad3DPoint originalPos = textEntity.FirstAlignment;
                                Cad3DPoint newPos = new Cad3DPoint(
                                    originalPos.X,
                                    originalPos.Y - 10,
                                    originalPos.Z);
                                textEntity.FirstAlignment = newPos;
                            }
                        }
                    }

                    // Prepare PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the modified drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine("Title position adjusted and saved to PDF.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}