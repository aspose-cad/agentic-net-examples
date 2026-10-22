using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace CadSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path
            string inputFile = "sample.dwg";

            // Verify input file exists
            if (!File.Exists(inputFile))
            {
                Console.WriteLine("Input file not found: " + inputFile);
                return;
            }

            // Prepare output directory and file
            string outputDir = "output";
            Directory.CreateDirectory(outputDir);
            string outputFile = Path.Combine(outputDir, "output.pdf");

            try
            {
                // Load the CAD image
                using (Image image = Image.Load(inputFile))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Iterate through CAD entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        Console.WriteLine($"Entity type: {entity.TypeName}");

                        // Example placeholder for hatch origin comparison
                        // if (entity.TypeName == "HATCH")
                        // {
                        //     // Access hatch-specific properties here (e.g., Origin)
                        //     // Compare original and new origin positions as needed
                        // }
                    }

                    // Set up rasterization options for PDF export
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter,
                        PageWidth = 21,   // A4 width in cm
                        PageHeight = 29.7f,  // A4 height in cm
                        AutomaticLayoutsScaling = true,
                        Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
                    };

                    // Configure PDF save options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the CAD drawing as PDF
                    cadImage.Save(outputFile, pdfOptions);
                }

                Console.WriteLine("Conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during processing: " + ex.Message);
            }
        }
    }
}