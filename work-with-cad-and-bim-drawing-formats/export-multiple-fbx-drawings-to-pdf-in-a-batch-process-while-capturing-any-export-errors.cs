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
            string inputFolder = "Input";
            string outputFolder = "Output";

            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            string[] fbxFiles = Directory.GetFiles(inputFolder, "*.fbx", SearchOption.TopDirectoryOnly);
            if (fbxFiles.Length == 0)
            {
                Console.WriteLine("No FBX files found to process.");
                return;
            }

            foreach (string inputPath in fbxFiles)
            {
                try
                {
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(inputPath);
                    string outputPath = Path.Combine(outputFolder, fileNameWithoutExt + ".pdf");

                    using (Image cadImage = Image.Load(inputPath))
                    {
                        PdfOptions pdfOptions = new PdfOptions
                        {
                            VectorRasterizationOptions = new CadRasterizationOptions
                            {
                                // Optional: set page size or other rasterization settings here
                                // PageSize = new System.Drawing.SizeF(800, 600)
                            }
                        };

                        cadImage.Save(outputPath, pdfOptions);
                        Console.WriteLine($"Successfully exported: {inputPath} -> {outputPath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error exporting file '{inputPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}