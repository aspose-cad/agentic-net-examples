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
        const string externalPath = "external.dwg";
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
                var mainCad = (CadImage)mainImage;

                // Load the external CAD drawing to be inserted
                using (Image externalImage = Image.Load(externalPath))
                {
                    var externalCad = (CadImage)externalImage;

                    // Create a block definition that will hold the external drawing's entities
                    var externalBlock = new CadBlockEntity
                    {
                        Name = "ExternalBlock"
                    };

                    // Copy all entities from the external drawing into the new block
                    foreach (var entity in externalCad.Entities)
                    {
                        externalBlock.AddEntity(entity);
                    }

                    // Add the block definition to the main drawing's block table
                    // BlockEntities is a dictionary keyed by block name
                    mainCad.BlockEntities.Add(externalBlock.Name, externalBlock);

                    // Insert the block into the main drawing at the required offset
                    var insert = new CadInsertObject
                    {
                        Name = externalBlock.Name,
                        InsertionPoint = new Cad3DPoint(100.0, 200.0, 0.0) // offsets to align titles
                    };

                    // Append the insert object to the main drawing's entities
                    mainCad.Entities = mainCad.Entities
                        .Concat(new CadEntityBase[] { insert })
                        .ToArray();
                }

                // Save the modified drawing as PDF
                var pdfOptions = new PdfOptions();
                mainCad.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}