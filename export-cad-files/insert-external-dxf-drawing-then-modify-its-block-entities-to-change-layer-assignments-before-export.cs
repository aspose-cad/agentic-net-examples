using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string mainPath = "main.dwg";
        const string externalPath = "external.dxf";
        const string outputPath = "output.pdf";

        if (!File.Exists(mainPath))
        {
            Console.Error.WriteLine($"File not found: {mainPath}");
            return;
        }

        if (!File.Exists(externalPath))
        {
            Console.Error.WriteLine($"File not found: {externalPath}");
            return;
        }

        try
        {
            // Load the main CAD drawing
            using (Image mainImage = Image.Load(mainPath))
            {
                CadImage mainCad = (CadImage)mainImage;

                // Load the external DXF drawing that will be inserted
                using (Image externalImage = Image.Load(externalPath))
                {
                    CadImage externalCad = (CadImage)externalImage;

                    // Iterate through each block definition in the external drawing
                    foreach (CadBlockEntity block in externalCad.BlockEntities.ValuesTyped)
                    {
                        // Change the layer assignment of every entity inside the block
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            entity.LayerName = "NewLayer";
                        }

                        // Add the modified block entities to the main drawing
                        mainCad.Entities = mainCad.Entities.Concat(block.Entities).ToArray();
                    }
                }

                // Export the modified drawing to PDF
                PdfOptions pdfOptions = new PdfOptions();
                mainCad.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}