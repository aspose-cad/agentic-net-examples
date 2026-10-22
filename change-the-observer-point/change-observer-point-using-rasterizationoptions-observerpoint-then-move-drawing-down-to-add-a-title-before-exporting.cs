using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the source CAD file path. Replace with an actual file location as needed.
            string sourcePath = "input.dxf";

            // Guard: ensure the source file exists.
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Define the destination file path where the rendered PDF will be saved.
            string destinationPath = "output.pdf";

            // Ensure the output directory exists.
            string outputDir = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD document.
            using (Image cadImage = Image.Load(sourcePath))
            {
                CadImage cad = cadImage as CadImage;
                if (cad == null)
                {
                    Console.WriteLine("The loaded file is not a recognized CAD format.");
                    return;
                }

                // Configure rasterization options.
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Observer point (rotation angles in degrees).
                    ObserverPoint = new ObserverPoint(30f, 45f, 0f),

                    // Page size (A4 in points).
                    PageWidth = 842,
                    PageHeight = 595,

                    // Automatic scaling of layouts.
                    AutomaticLayoutsScaling = true,

                    // Increase top margin to create space for a title.
                    Margins = new Margins
                    {
                        Left = 10,
                        Right = 10,
                        Top = 50,   // extra space for title
                        Bottom = 10
                    },

                    // Line scaling.
                    LineScale = 1.0f
                };

                // PDF export configuration.
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD image to PDF.
                cad.Save(destinationPath, pdfOptions);
            }

            Console.WriteLine("CAD file has been rendered to PDF with a custom observer point and top margin for title.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}