using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.dwg";
        string outputDirectory = "output";
        string outputPath = Path.Combine(outputDirectory, "result.pdf");

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Input file does not exist: " + inputPath);
            return;
        }

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Create a new line entity and assign it to a specific layer
                CadLine lineEntity = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 100, 0),
                    LayerName = "MyLayer"
                };

                // Add the entity to the ModelSpace block
                if (cadImage.BlockEntities.ContainsKey("ModelSpace"))
                {
                    cadImage.BlockEntities["ModelSpace"].AddEntity(lineEntity);
                }

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter, // SDK spelling
                    LineScale = 1.0f,
                    PageWidth = 800,
                    PageHeight = 600
                };

                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD file as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine("Export completed successfully: " + outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}