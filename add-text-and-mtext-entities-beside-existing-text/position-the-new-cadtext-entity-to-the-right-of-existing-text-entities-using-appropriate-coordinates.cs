using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadTextAdditionExample
{
    class Program
    {
        static void Main(string[] args)
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
                // Load the DWG/DXF file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Get the Model_Space block which contains drawing entities
                    CadBlockEntity modelSpace = cadImage.BlockEntities["Model_Space"];

                    // Determine the rightmost X coordinate among existing CadText entities
                    double maxX = 0.0;
                    foreach (CadEntityBase entity in modelSpace.Entities)
                    {
                        if (entity is CadText existingText && existingText.FirstAlignment != null)
                        {
                            double x = existingText.FirstAlignment.X;
                            if (x > maxX)
                            {
                                maxX = x;
                            }
                        }
                    }

                    // Create a new CadText entity positioned to the right of existing texts
                    CadText newText = new CadText
                    {
                        DefaultValue = "New Text",
                        TextHeight = 2.5,
                        FirstAlignment = new Cad3DPoint(maxX + 10.0, 0.0, 0.0)
                    };

                    // Add the new text entity to the Model_Space block
                    modelSpace.AddEntity(newText);

                    // Prepare PDF save options
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            UnitType = UnitType.Centimenter,
                            LineScale = 1f
                        }
                    };

                    // Save the modified drawing to PDF
                    cadImage.Save(outputPath, pdfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}