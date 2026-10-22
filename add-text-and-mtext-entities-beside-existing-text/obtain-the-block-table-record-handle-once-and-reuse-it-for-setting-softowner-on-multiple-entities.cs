using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output/output.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            using (Image img = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)img;

                // Obtain the Model Space block once
                CadBlockEntity modelSpace = cadImage.BlockEntities
                    .Values
                    .OfType<CadBlockEntity>()
                    .FirstOrDefault(b => b.Name == "*Model_Space");

                if (modelSpace == null)
                {
                    Console.WriteLine("Model space block not found.");
                    return;
                }

                // Get the block table record handle (SoftOwner) from the Model layout
                string blockHandle = cadImage.Layouts
                    .Values
                    .OfType<CadLayout>()
                    .FirstOrDefault(l => l.LayoutName.Equals("Model", StringComparison.OrdinalIgnoreCase))
                    ?.BlockTableRecordHandle;

                // Fallback: use the first layout's handle if specific Model layout not found
                if (string.IsNullOrEmpty(blockHandle) && cadImage.Layouts.Count > 0)
                {
                    blockHandle = cadImage.Layouts.Values
                        .OfType<CadLayout>()
                        .First()
                        .BlockTableRecordHandle;
                }

                if (string.IsNullOrEmpty(blockHandle))
                {
                    Console.WriteLine("Unable to obtain block table record handle.");
                    return;
                }

                // Create a TEXT entity
                CadText textEntity = new CadText
                {
                    DefaultValue = "Hello Text",
                    FirstAlignment = new Cad3DPoint(10, 10, 0),
                    TextHeight = 2.5,
                    LayerName = "0",
                    SoftOwner = blockHandle
                };

                // Create an MTEXT entity
                CadMText mtextEntity = new CadMText
                {
                    Text = "Hello MText",
                    InsertionPoint = new Cad3DPoint(10, 15, 0),
                    InitialTextHeight = 2.5,
                    LayerName = "0",
                    SoftOwner = blockHandle
                };

                // Add entities to the Model Space block
                modelSpace.AddEntity(textEntity);
                modelSpace.AddEntity(mtextEntity);

                // Save the modified drawing
                cadImage.Save(outputPath);
            }

            Console.WriteLine("TEXT and MTEXT entities added successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}