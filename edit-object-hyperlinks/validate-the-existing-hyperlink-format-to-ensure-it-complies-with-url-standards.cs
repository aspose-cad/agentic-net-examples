using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
                // Hyperlink to validate
                string hyperlink = "https://example.com/path?query=1";

                // Validate URL format
                bool isValid = Uri.IsWellFormedUriString(hyperlink, UriKind.Absolute);
                Console.WriteLine($"Hyperlink \"{hyperlink}\" is {(isValid ? "valid" : "invalid")}.");

                // Demonstrate Aspose.CAD usage: load a CAD file and convert to PDF if the file exists
                string cadFilePath = "sample.dwg";
                if (System.IO.File.Exists(cadFilePath))
                {
                    using (Image cadImage = Image.Load(cadFilePath))
                    {
                        string pdfOutputPath = "output.pdf";
                
                    string outputDir = Path.GetDirectoryName(pdfOutputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
        PdfOptions pdfOptions = new PdfOptions();
                        cadImage.Save(pdfOutputPath, pdfOptions);
                        Console.WriteLine($"CAD file converted to PDF: {pdfOutputPath}");
                    }
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}