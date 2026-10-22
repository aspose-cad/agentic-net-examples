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
            Console.WriteLine($"Input file \"{inputPath}\" not found.");
            return;
        }

        try
        {
            // Load the DWG/DXF file
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Find the Model layout
                CadLayout modelLayout = null;
                foreach (var kvp in cadImage.Layouts)
                {
                    var layout = kvp.Value;
                    if (!string.IsNullOrEmpty(layout.LayoutName) &&
                        layout.LayoutName.Equals("*Model_Space", StringComparison.OrdinalIgnoreCase))
                    {
                        modelLayout = layout;
                        break;
                    }
                }

                if (modelLayout == null)
                {
                    Console.WriteLine("Model layout not found.");
                    return;
                }

                // Get the Model Space block using the layout's BlockTableRecordHandle
                if (!cadImage.BlockEntities.TryGetValue(modelLayout.BlockTableRecordHandle, out CadBlockEntity modelSpaceBlock))
                {
                    Console.WriteLine("Model space block not found.");
                    return;
                }

                // Create a new MText entity with SoftOwner set to the model layout's block table record handle
                CadMText newMText = new CadMText
                {
                    Text = "Added MText",
                    InsertionPoint = new Cad3DPoint(10.0, 10.0, 0.0),
                    InitialTextHeight = 2.5,
                    SoftOwner = modelLayout.BlockTableRecordHandle
                };

                // Optionally, also add a simple Text entity
                CadText newText = new CadText
                {
                    DefaultValue = "Added Text",
                    FirstAlignment = new Cad3DPoint(10.0, 12.0, 0.0),
                    TextHeight = 2.5,
                    SoftOwner = modelLayout.BlockTableRecordHandle
                };

                // Add the entities to the Model Space block
                modelSpaceBlock.AddEntity(newMText);
                modelSpaceBlock.AddEntity(newText);

                // Save the modified drawing to PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Successfully saved modified drawing to \"{outputPath}\".");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}