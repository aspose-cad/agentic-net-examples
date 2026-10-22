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
                string inputPath = "input.dwg";
                string outputPath = "output.pdf";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.PageWidth = 1600;
                    rasterOptions.PageHeight = 1200;
                    rasterOptions.Layouts = new string[] { "Model" };
                    rasterOptions.DrawType = CadDrawTypeMode.UseObjectColor;

                    // Set up PDF save options with the rasterization settings
                    PdfOptions pdfOptions = new PdfOptions();
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the CAD file as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"CAD file converted to PDF: {outputPath}");
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}