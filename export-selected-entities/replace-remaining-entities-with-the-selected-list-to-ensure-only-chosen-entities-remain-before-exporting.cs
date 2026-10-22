using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadEntityFilterExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "input.dwg";
                // Path for the filtered output PDF
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

                    // Define the list of entity types to keep (e.g., only lines and circles)
                    HashSet<Type> allowedTypes = new HashSet<Type>
                    {
                        typeof(CadLine),
                        typeof(CadCircle)
                    };

                    // Filter entities to keep only allowed types
                    var filteredEntities = cadImage.Entities
                        .Where(e => allowedTypes.Contains(e.GetType()))
                        .ToArray();

                    // Replace the Entities collection with the filtered set
                    cadImage.Entities = filteredEntities;

                    // Prepare PDF save options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the filtered drawing to PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine("Filtered CAD file has been saved to " + outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}