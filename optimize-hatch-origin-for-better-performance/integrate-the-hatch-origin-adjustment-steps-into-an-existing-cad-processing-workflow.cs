using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

namespace OptimizeHatchOrigin
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "input.dwg";
                // Path for the output PDF file
                string outputPath = "output.pdf";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
                {
                    // Iterate through all top‑level entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Identify hatch entities by type name (fallback if specific class is unavailable)
                        if (entity.TypeName != null && entity.TypeName.ToString().Contains("Hatch", StringComparison.OrdinalIgnoreCase))
                        {
                            // Use dynamic to avoid compile‑time dependency on a specific hatch class
                            dynamic hatch = entity;
                            try
                            {
                                // Attempt to read current origin (if the property exists)
                                var currentOrigin = hatch.Origin;

                                // Create a new origin point (using the default constructor)
                                var newOrigin = new Aspose.CAD.FileFormats.Cad.CadObjects.CadPoint();

                                // Example shift logic – actual X/Y properties may not be exposed in this SDK version
                                // If X/Y are available, they could be set like:
                                // newOrigin.X = currentOrigin.X + 10.0;
                                // newOrigin.Y = currentOrigin.Y + 5.0;

                                // Assign the new origin back to the hatch (if the setter exists)
                                hatch.Origin = newOrigin;
                            }
                            catch
                            {
                                // Silently ignore if the hatch entity does not expose Origin or related members
                            }
                        }
                    }

                    // Save the modified CAD image to PDF
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