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
            string inputFile = "sample.dwf";
            if (!File.Exists(inputFile))
            {
                Console.Error.WriteLine($"Input file not found: {inputFile}");
                return;
            }

            string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            using (Image cadImage = Image.Load(inputFile))
            {
                // PNG
                string pngPath = Path.Combine(outputDir, "output.png");
                PngOptions pngOptions = new PngOptions();
                cadImage.Save(pngPath, pngOptions);

                // JPEG
                string jpegPath = Path.Combine(outputDir, "output.jpg");
                JpegOptions jpegOptions = new JpegOptions();
                cadImage.Save(jpegPath, jpegOptions);

                // BMP
                string bmpPath = Path.Combine(outputDir, "output.bmp");
                BmpOptions bmpOptions = new BmpOptions();
                cadImage.Save(bmpPath, bmpOptions);

                // TIFF
                string tiffPath = Path.Combine(outputDir, "output.tiff");
                TiffOptions tiffOptions = new TiffOptions();
                cadImage.Save(tiffPath, tiffOptions);

                // SVG
                string svgPath = Path.Combine(outputDir, "output.svg");
                SvgOptions svgOptions = new SvgOptions();
                cadImage.Save(svgPath, svgOptions);

                // PDF
                string pdfPath = Path.Combine(outputDir, "output.pdf");
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(pdfPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}