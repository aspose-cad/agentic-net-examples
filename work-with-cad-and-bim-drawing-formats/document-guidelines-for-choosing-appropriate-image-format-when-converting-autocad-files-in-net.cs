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
            // Input CAD file path (replace with actual file)
            string inputPath = "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // -------------------------------------------------
                // Guideline 1: Use vector formats when you need
                //            to preserve geometry, layers, or
                //            layout information.
                // -------------------------------------------------

                // Export to PDF (vector format)
                string pdfPath = "output.pdf";
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(pdfPath, pdfOptions);
                Console.WriteLine($"Saved PDF: {pdfPath}");

                // Export to SVG (vector format suitable for web)
                string svgPath = "output.svg";
                SvgOptions svgOptions = new SvgOptions();
                cadImage.Save(svgPath, svgOptions);
                Console.WriteLine($"Saved SVG: {svgPath}");

                // -------------------------------------------------
                // Guideline 2: Use raster formats when you need
                //            pixel‑based rendering, thumbnails,
                //            or when the target environment only
                //            supports raster images.
                // -------------------------------------------------

                // Export to PNG (raster format, lossless)
                string pngPath = "output.png";
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        // Optional: specify layout(s) to render
                        Layouts = new string[] { "Model" }
                    }
                };
                cadImage.Save(pngPath, pngOptions);
                Console.WriteLine($"Saved PNG: {pngPath}");

                // Export to JPEG (raster format, lossy compression)
                string jpegPath = "output.jpg";
                JpegOptions jpegOptions = new JpegOptions
                {
                    Quality = 90,
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200
                    }
                };
                cadImage.Save(jpegPath, jpegOptions);
                Console.WriteLine($"Saved JPEG: {jpegPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}