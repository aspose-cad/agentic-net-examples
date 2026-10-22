using Aspose.CAD;
using System;
using System.Linq;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace CadMTextExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Create a new empty CAD image
                CadImage cadImage = new CadImage();

                // Ensure there is at least one layout (model space)
                if (!cadImage.Layouts.Any())
                {
                    Console.WriteLine("No layouts available in the CAD image.");
                    return;
                }

                // Retrieve the model space layout (first layout is typically model space)
                CadLayout modelSpaceLayout = cadImage.Layouts.Values.First();

                // Prepare a multiline MTEXT entity
                CadMText mtext = new CadMText
                {
                    Text = "First line\\nSecond line\\nThird line",
                    InsertionPoint = new Cad3DPoint(0.0, 0.0, 0.0),
                    InitialTextHeight = 2.5,
                    RotationAngleRad = 0.0,
                    ReferenceRectangleWidth = 100.0,
                    SoftOwner = modelSpaceLayout.BlockTableRecordHandle
                };

                // Add the MTEXT entity to the model space block
                CadBlockEntity modelSpaceBlock = cadImage.BlockEntities[modelSpaceLayout.BlockTableRecordHandle];
                modelSpaceBlock.AddEntity(mtext);

                // Append the new entity to the image's entity collection via replacement
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { mtext }).ToArray();

                // Prepare output directory
                string outputDir = "Output";
                System.IO.Directory.CreateDirectory(outputDir);
                string outputPath = System.IO.Path.Combine(outputDir, "output.dxf");

                // Save the CAD file preserving editable objects
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary,
                    Version = DxfOutputVersion.R12
                };

                cadImage.Save(outputPath);
                Console.WriteLine($"CAD file saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}