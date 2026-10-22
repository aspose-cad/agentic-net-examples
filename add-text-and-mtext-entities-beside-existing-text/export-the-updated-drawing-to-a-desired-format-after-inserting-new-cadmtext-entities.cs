using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                // Create a new MText entity
                var mText = new CadMText
                {
                    Text = "Hello, Aspose.CAD!",
                    InsertionPoint = new Cad3DPoint(0, 0, 0)
                };

                // Add MText to Model Space
                if (cadImage.BlockEntities.TryGetValue("*Model_Space", out var modelSpace))
                {
                    modelSpace.Entities.Add(mText);
                }
                else
                {
                    Console.Error.WriteLine("Model space not found in the drawing.");
                }

                // Save the updated drawing to PDF format
                var pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}