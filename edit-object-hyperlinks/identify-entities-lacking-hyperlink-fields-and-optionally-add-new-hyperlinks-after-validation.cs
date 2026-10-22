using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadHyperlinkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "input.dwg";
                // Path for the exported PDF file
                string outputPath = "output.pdf";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Process top‑level drawing entities
                    foreach (var entity in cadImage.Entities)
                    {
                        // If the entity does not have a hyperlink, add one
                        if (string.IsNullOrEmpty(entity.Hyperlink))
                        {
                            entity.Hyperlink = "http://example.com";
                        }
                    }

                    // Process block definitions and their inner entities
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (var blockEntity in block.Entities)
                        {
                            if (string.IsNullOrEmpty(blockEntity.Hyperlink))
                            {
                                blockEntity.Hyperlink = "http://example.com";
                            }
                        }
                    }

                    // Save the modified drawing as PDF
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPath, pdfOptions);
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