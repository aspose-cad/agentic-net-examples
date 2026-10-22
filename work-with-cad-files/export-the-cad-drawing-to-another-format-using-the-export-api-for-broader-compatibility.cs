using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                string extension = Path.GetExtension(outputPath).ToLowerInvariant();

                if (extension == ".pdf")
                {
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPath, pdfOptions);
                }
                else if (extension == ".svg")
                {
                    SvgOptions svgOptions = new SvgOptions();
                    cadImage.Save(outputPath, svgOptions);
                }
                else if (extension == ".png")
                {
                    PngOptions pngOptions = new PngOptions();
                    cadImage.Save(outputPath, pngOptions);
                }
                else if (extension == ".jpeg" || extension == ".jpg")
                {
                    JpegOptions jpegOptions = new JpegOptions();
                    cadImage.Save(outputPath, jpegOptions);
                }
                else
                {
                    Console.Error.WriteLine($"Unsupported output format: {extension}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}