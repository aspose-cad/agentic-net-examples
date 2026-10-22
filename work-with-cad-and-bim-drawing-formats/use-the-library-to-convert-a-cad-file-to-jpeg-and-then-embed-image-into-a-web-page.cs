using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string jpegPath = "output.jpg";
        string htmlPath = "output.html";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                JpegOptions jpegOptions = new JpegOptions();
                jpegOptions.Quality = 90;
                cadImage.Save(jpegPath, jpegOptions);
            }

            if (!File.Exists(jpegPath))
            {
                Console.Error.WriteLine($"Failed to create JPEG file: {jpegPath}");
                return;
            }

            byte[] jpegBytes = File.ReadAllBytes(jpegPath);
            string base64Image = Convert.ToBase64String(jpegBytes);

            string htmlContent = "<!DOCTYPE html>\n" +
                                 "<html>\n" +
                                 "<head><title>CAD to JPEG</title></head>\n" +
                                 "<body>\n" +
                                 "<h1>CAD to JPEG Conversion</h1>\n" +
                                 $"<img src=\"data:image/jpeg;base64,{base64Image}\" alt=\"CAD Image\" />\n" +
                                 "</body>\n" +
                                 "</html>";

            File.WriteAllText(htmlPath, htmlContent);
            Console.WriteLine($"Conversion completed. JPEG saved to '{jpegPath}'. HTML page saved to '{htmlPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}