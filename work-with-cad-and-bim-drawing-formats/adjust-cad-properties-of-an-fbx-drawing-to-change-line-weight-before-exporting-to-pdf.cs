using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.fbx";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            var outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the FBX drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Desired line weight (in millimeters, for example)
                float newLineWeight = 0.5f;

                // NOTE: Aspose.CAD's CadLine entity does not expose a LineWeight property.
                // Changing line weight directly is not supported by the current SDK.
                // The code below demonstrates where such logic would be placed if the API provided it.

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLine cadLine)
                    {
                        // LineWeight property is unavailable; cannot modify line weight.
                        // Placeholder for future implementation:
                        // cadLine.LineWeight = newLineWeight;
                    }
                    else if (entity is CadInsertObject insertObject)
                    {
                        AdjustLineWeightInInsert(insertObject, cadImage, newLineWeight);
                    }
                }

                // Export the (unmodified) drawing to PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
    }

    static void AdjustLineWeightInInsert(CadInsertObject insertObject, CadImage cadImage, float newWeight)
    {
        // Find the block definition that corresponds to this insert reference
        CadBlockEntity block = cadImage.BlockEntities.ValuesTyped.FirstOrDefault(b => b.Name == insertObject.Name);
        if (block == null)
            return;

        // Adjust line weight for entities inside the block
        foreach (CadEntityBase innerEntity in block.Entities)
        {
            if (innerEntity is CadLine innerLine)
            {
                // LineWeight property is unavailable; cannot modify line weight.
                // Placeholder for future implementation:
                // innerLine.LineWeight = newWeight;
            }
            else if (innerEntity is CadInsertObject innerInsert)
            {
                // Recursively handle nested block inserts
                AdjustLineWeightInInsert(innerInsert, cadImage, newWeight);
            }
        }
    }
}