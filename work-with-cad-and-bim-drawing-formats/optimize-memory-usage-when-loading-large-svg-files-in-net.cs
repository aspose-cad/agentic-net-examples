using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Paths for input SVG and output PDF
        string inputSvgPath = "large_input.svg";
        string outputPdfPath = "output.pdf";

        // Validate input file existence
        if (!File.Exists(inputSvgPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputSvgPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPdfPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Open the SVG file with a FileStream to keep memory usage low
            using (FileStream svgStream = new FileStream(inputSvgPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // Load the SVG as a CadImage
                using (CadImage cadImage = (CadImage)Image.Load(svgStream))
                {
                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter,
                        LineScale = 1.0f,
                        PageWidth = 800,
                        PageHeight = 600,
                        AutomaticLayoutsScaling = true,
                        Margins = new Margins
                        {
                            Left = 10,
                            Right = 10,
                            Top = 10,
                            Bottom = 10
                        },
                        Layouts = new string[] { "Model" }
                    };

                    // Prepare PDF save options and assign rasterization settings
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the image to PDF, releasing resources after the using block
                    cadImage.Save(outputPdfPath, pdfOptions);
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