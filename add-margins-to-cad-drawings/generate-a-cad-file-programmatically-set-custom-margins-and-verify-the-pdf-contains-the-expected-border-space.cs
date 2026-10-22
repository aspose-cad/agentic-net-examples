using System;
using System.IO;
using System.Text;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output paths
            string inputPath = "sample.dwg";
            string outputDir = "Output";
            string outputPath = Path.Combine(outputDir, "result.pdf");

            // Verify input CAD file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input CAD file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options (using defaults)
            var rasterOptions = new CadRasterizationOptions
            {
                // Render the default layout (e.g., "Model")
                Layouts = new[] { "Model" }
            };

            // Configure PDF save options
            var pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
                // Note: PageMargins property is not available in the current Aspose.CAD version.
                // Margins can be simulated by adjusting the layout or scaling if needed.
            };

            // Save the CAD drawing as PDF with the specified options
            cadImage.Save(outputPath, pdfOptions);

            // Verify that the PDF file was created and contains the expected header
            if (!File.Exists(outputPath))
            {
                Console.Error.WriteLine("Failed to create PDF output.");
                return;
            }

            // Read first 4 bytes to check PDF signature
            using (FileStream fs = new FileStream(outputPath, FileMode.Open, FileAccess.Read))
            {
                byte[] header = new byte[4];
                int bytesRead = fs.Read(header, 0, header.Length);
                string headerString = Encoding.ASCII.GetString(header, 0, bytesRead);
                if (headerString.StartsWith("%PDF"))
                {
                    Console.WriteLine("PDF generated successfully.");
                }
                else
                {
                    Console.Error.WriteLine("Generated file does not appear to be a valid PDF.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
