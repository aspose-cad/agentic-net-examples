using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "sample.stp";
        string vectorPdfPath = "output_vector.pdf";
        string rasterPdfPath = "output_raster.pdf";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the STEP file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Export as vector PDF (no rasterization)
                PdfOptions vectorOptions = new PdfOptions();
                cadImage.Save(vectorPdfPath, vectorOptions);

                // Export as raster PDF (force rasterization via CadRasterizationOptions)
                PdfOptions rasterOptions = new PdfOptions();
                rasterOptions.VectorRasterizationOptions = new CadRasterizationOptions
                {
                    // Define raster page size (in pixels) and resolution
                    PageWidth = 1200,
                    PageHeight = 800,
                    // Optional: set background color, layout, etc.
                };
                cadImage.Save(rasterPdfPath, rasterOptions);
            }

            // Get file sizes
            long vectorSize = new FileInfo(vectorPdfPath).Length;
            long rasterSize = new FileInfo(rasterPdfPath).Length;

            Console.WriteLine($"Vector PDF size: {vectorSize} bytes");
            Console.WriteLine($"Raster PDF size: {rasterSize} bytes");
            Console.WriteLine($"Size difference: {Math.Abs(rasterSize - vectorSize)} bytes");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}