using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            string inputFolder = @"C:\InputObjs";
            string outputFolder = @"C:\OutputPdfs";

            Directory.CreateDirectory(outputFolder);

            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            string[] objFiles = Directory.GetFiles(inputFolder, "*.obj");

            foreach (string objFilePath in objFiles)
            {
                if (!File.Exists(objFilePath))
                {
                    Console.Error.WriteLine($"File not found: {objFilePath}");
                    continue;
                }

                using (Image cadImage = Image.Load(objFilePath))
                {
                    // Adjust CAD properties here if needed (OBJ format has limited editable properties)

                    PdfOptions pdfOptions = new PdfOptions();

                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1200f,
                        PageHeight = 800f,
                        UnitType = UnitType.Centimenter,
                        LineScale = 1.0f,
                        AutomaticLayoutsScaling = true,
                        Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
                        // Layouts can be set via the Layouts property if specific layout names are required
                    };

                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    string outputFileName = Path.GetFileNameWithoutExtension(objFilePath) + ".pdf";
                    string outputPath = Path.Combine(outputFolder, outputFileName);

                    cadImage.Save(outputPath, pdfOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}