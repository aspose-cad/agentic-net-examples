using System;
using System.Diagnostics;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the source DXF file
                string inputPath = "input.dxf";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the DXF drawing
                using (Image dxfImage = Image.Load(inputPath))
                {
                    // Export to WMF and measure rendering time
                    string wmfPath = "output.wmf";
                    WmfOptions wmfOptions = new WmfOptions();
                    Stopwatch wmfStopwatch = new Stopwatch();
                    wmfStopwatch.Start();
                    dxfImage.Save(wmfPath, wmfOptions);
                    wmfStopwatch.Stop();
                    Console.WriteLine("WMF export time: " + wmfStopwatch.ElapsedMilliseconds + " ms");

                    // Export to PDF and measure rendering time
                    string pdfPath = "output.pdf";
                    PdfOptions pdfOptions = new PdfOptions();
                    Stopwatch pdfStopwatch = new Stopwatch();
                    pdfStopwatch.Start();
                    dxfImage.Save(pdfPath, pdfOptions);
                    pdfStopwatch.Stop();
                    Console.WriteLine("PDF export time: " + pdfStopwatch.ElapsedMilliseconds + " ms");
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}