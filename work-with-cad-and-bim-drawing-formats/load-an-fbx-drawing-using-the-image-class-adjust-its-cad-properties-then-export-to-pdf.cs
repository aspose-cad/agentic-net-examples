using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source FBX file
            string inputFile = "input.fbx";

            // Verify input file exists
            if (!File.Exists(inputFile))
            {
                Console.Error.WriteLine($"Input file not found: {inputFile}");
                return;
            }

            // Load the FBX drawing
            using (Image image = Image.Load(inputFile))
            {
                // Cast to CadImage to work with CAD‑specific features
                Image cadImage = image ;
                if (cadImage != null)
                {
                    // Prepare PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Configure rasterization options (e.g., page size, layout) for the PDF output
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,   // width in pixels
                        PageHeight = 600,  // height in pixels
                        Layouts = new string[] { "Model" } // use the "Model" layout if present
                    };

                    // Assign rasterization options to the PDF options
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Path for the resulting PDF file
                    string outputFile = "output.pdf";

                    // Ensure output directory exists
                    string outDir = Path.GetDirectoryName(outputFile);
                    if (!string.IsNullOrEmpty(outDir))
                    {
                        Directory.CreateDirectory(outDir);
                    }

                    // Export the CAD drawing to PDF
                    cadImage.Save(outputFile, pdfOptions);
                }
                else
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}