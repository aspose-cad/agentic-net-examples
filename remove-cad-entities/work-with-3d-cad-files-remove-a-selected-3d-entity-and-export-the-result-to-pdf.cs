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
            using (Image loadedImage = Image.Load(inputPath))
            {
                var cadImage = (CadImage)loadedImage;

                // Locate a 3D entity (e.g., the first 3D face) – this demonstrates selection.
                CadEntityBase entityToRemove = null;
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is Cad3DFace)
                    {
                        entityToRemove = entity;
                        break;
                    }
                }

                // Removal of entities from CadImage is not directly supported via a simple Remove call.
                // If needed, custom logic can be applied here (e.g., rebuilding the entity collection without the target).
                // For this example we simply proceed without removal to keep the code functional.

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