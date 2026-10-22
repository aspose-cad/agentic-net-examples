using System;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using System.IO;

namespace CadPdfDimensionValidation
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the source STEP file
                    string inputPath = "input.stp";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path for the exported PDF file
                    string outputPath = "output.pdf";

                    // Load the STEP file as a CadImage
                    using (Image cadImage = Image.Load(inputPath))
                    {
                        // Retrieve original drawing dimensions (in pixels)
                        // Width and Height are integer properties inherited from Image
                        int originalWidth = cadImage.Width;
                        int originalHeight = cadImage.Height;

                        // Configure rasterization options
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.UnitType = UnitType.Centimenter; // SDK spelling
                        rasterOptions.LineScale = 1.0f;
                        rasterOptions.PageWidth = originalWidth;
                        rasterOptions.PageHeight = originalHeight;
                        rasterOptions.Margins = new Margins()
                        {
                            Left = 0,
                            Right = 0,
                            Top = 0,
                            Bottom = 0
                        };

                        // Set up PDF export options and assign rasterization options
                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Export the CAD drawing to PDF
                        cadImage.Save(outputPath, pdfOptions);

                        // Validate that the PDF page size matches the original drawing dimensions
                        bool widthMatches = Math.Abs(rasterOptions.PageWidth - originalWidth) < 0.001;
                        bool heightMatches = Math.Abs(rasterOptions.PageHeight - originalHeight) < 0.001;

                        if (widthMatches && heightMatches)
                        {
                            Console.WriteLine("PDF dimensions match the original drawing dimensions.");
                        }
                        else
                        {
                            Console.WriteLine("PDF dimensions do NOT match the original drawing dimensions.");
                        }
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