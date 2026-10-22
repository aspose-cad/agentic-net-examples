using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputFolder = "InputFiles";
            string outputFolder = "OutputFiles";

            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            string[] colladaFiles = Directory.GetFiles(inputFolder, "*.dae");
            if (colladaFiles.Length == 0)
            {
                Console.Error.WriteLine("No COLLADA (.dae) files found in the input folder.");
                return;
            }

            foreach (string inputPath in colladaFiles)
            {
                try
                {
                    using (Image cadImage = Image.Load(inputPath))
                    {
                        var pdfOptions = new PdfOptions();

                        var rasterOptions = new CadRasterizationOptions
                        {
                            PageWidth = 800,
                            PageHeight = 600
                            // Layouts can be set if needed, e.g., Layouts = new[] { "Model" };
                        };
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        string outputFileName = Path.GetFileNameWithoutExtension(inputPath) + ".pdf";
                        string outputPath = Path.Combine(outputFolder, outputFileName);

                        cadImage.Save(outputPath, pdfOptions);
                        Console.WriteLine($"Converted '{inputPath}' to PDF successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{inputPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}