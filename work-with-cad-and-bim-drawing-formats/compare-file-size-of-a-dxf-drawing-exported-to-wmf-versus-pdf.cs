using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source DXF file
            string sourcePath = "input.dxf";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the DXF drawing
            using (Image dxfImage = Image.Load(sourcePath))
            {
                // Prepare output paths
                string wmfPath = "output.wmf";
                string pdfPath = "output.pdf";

                // Ensure output directories exist
                string wmfDir = Path.GetDirectoryName(wmfPath);
                if (!string.IsNullOrEmpty(wmfDir))
                    Directory.CreateDirectory(wmfDir);

                string pdfDir = Path.GetDirectoryName(pdfPath);
                if (!string.IsNullOrEmpty(pdfDir))
                    Directory.CreateDirectory(pdfDir);

                // Save the drawing as WMF
                WmfOptions wmfOptions = new WmfOptions();
                dxfImage.Save(wmfPath, wmfOptions);

                // Save the drawing as PDF
                PdfOptions pdfOptions = new PdfOptions();
                dxfImage.Save(pdfPath, pdfOptions);
            }

            // Retrieve file sizes
            FileInfo wmfInfo = new FileInfo("output.wmf");
            FileInfo pdfInfo = new FileInfo("output.pdf");

            long wmfSize = wmfInfo.Length;
            long pdfSize = pdfInfo.Length;

            // Output the sizes
            Console.WriteLine($"WMF file size: {wmfSize} bytes");
            Console.WriteLine($"PDF file size: {pdfSize} bytes");

            // Compare sizes
            if (wmfSize > pdfSize)
                Console.WriteLine("WMF is larger than PDF.");
            else if (wmfSize < pdfSize)
                Console.WriteLine("PDF is larger than WMF.");
            else
                Console.WriteLine("Both files have the same size.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}