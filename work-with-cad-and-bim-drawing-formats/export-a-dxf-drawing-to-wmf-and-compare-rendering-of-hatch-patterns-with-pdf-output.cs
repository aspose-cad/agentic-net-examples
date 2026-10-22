using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the source DXF file
                string sourcePath = "sample.dxf";

                // Verify that the DXF file exists before attempting to load it
                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the DXF drawing as a CadImage
                using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
                {
                    // -----------------------------------------------------------------
                    // Export to WMF (Windows Metafile) – preserves hatch patterns
                    // -----------------------------------------------------------------
                    var wmfOptions = new WmfOptions
                    {
                        // Configure rasterization options (size, background, etc.)
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1200,
                            // Preserve original colors, including hatch colors
                            BackgroundColor = Aspose.CAD.Color.White
                        }
                    };

                    string wmfOutput = "output.wmf";
            
                    string outputDir = Path.GetDirectoryName(wmfOutput);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
        cadImage.Save(wmfOutput, wmfOptions);
                    Console.WriteLine($"WMF file saved: {wmfOutput}");

                    // -----------------------------------------------------------------
                    // Export to PDF – also preserves hatch patterns for comparison
                    // -----------------------------------------------------------------
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1200,
                            BackgroundColor = Aspose.CAD.Color.White
                        }
                    };

                    string pdfOutput = "output.pdf";
                    cadImage.Save(pdfOutput, pdfOptions);
                    Console.WriteLine($"PDF file saved: {pdfOutput}");
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}