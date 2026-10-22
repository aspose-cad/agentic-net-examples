using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                // Define input CAD file path
                string inputCadPath = "sample.dwg";

                // Verify that the CAD file exists
                if (!File.Exists(inputCadPath))
                {
                    Console.WriteLine($"Input CAD file not found: {inputCadPath}");
                    return;
                }

                // Create output directory
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                Directory.CreateDirectory(outputDir);

                // Define output PDF path
                string outputPdfPath = Path.Combine(outputDir, "Converted.pdf");

                // Load CAD file and convert to PDF
                using (Image cadImage = Image.Load(inputCadPath))
                {
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPdfPath, pdfOptions);
                }

                // Create a simple HTML report that embeds the generated PDF
                string reportPath = Path.Combine(outputDir, "Report.html");
                string htmlContent = $"<html><body>" +
                                     $"<h1>CAD to PDF Conversion Report</h1>" +
                                     $"<p>Source CAD file: {Path.GetFileName(inputCadPath)}</p>" +
                                     $"<p>Converted PDF file: {Path.GetFileName(outputPdfPath)}</p>" +
                                     $"<embed src=\"{Path.GetFileName(outputPdfPath)}\" type=\"application/pdf\" width=\"100%\" height=\"600px\" />" +
                                     $"</body></html>";

                File.WriteAllText(reportPath, htmlContent);

                Console.WriteLine($"Conversion completed. PDF saved to: {outputPdfPath}");
                Console.WriteLine($"Report generated at: {reportPath}");
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}