using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadLayoutExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file (DWG/DXF)
                string sourcePath = "input.dwg";

                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD file
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cad = (CadImage)cadImage;

                    // List all layout names in the drawing
                    Console.WriteLine("Available Layouts:");
                    foreach (var layout in cad.Layouts.Values)
                    {
                        Console.WriteLine("- " + layout.LayoutName);
                    }

                    // Select a specific layout (for example, the first one)
                    var selectedLayout = cad.Layouts.Values.FirstOrDefault();
                    if (selectedLayout == null)
                    {
                        Console.WriteLine("No layouts found in the drawing.");
                        return;
                    }

                    string selectedLayoutName = selectedLayout.LayoutName;
                    Console.WriteLine($"Selected Layout: {selectedLayoutName}");

                    // Configure rasterization options to render only the selected layout
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = new[] { selectedLayoutName },
                        PageWidth = 1600,   // Desired output width
                        PageHeight = 1200   // Desired output height
                    };

                    // Choose PDF as the output format
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the selected layout to a PDF file
                    string outputPath = "selected_layout.pdf";
                    cad.Save(outputPath, pdfOptions);

                    Console.WriteLine($"Layout exported to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}