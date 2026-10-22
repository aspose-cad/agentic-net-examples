using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace AddMTextExample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "input.dxf";
            const string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the existing CAD file
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Find the Model space layout
                CadLayout modelLayout = cadImage.Layouts.Values
                    .FirstOrDefault(l => l.LayoutName.Equals("Model", StringComparison.OrdinalIgnoreCase));

                if (modelLayout == null)
                {
                    Console.Error.WriteLine("Model layout not found.");
                    return;
                }

                // Retrieve the block entity that corresponds to the Model space
                if (!cadImage.BlockEntities.TryGetValue(modelLayout.BlockTableRecordHandle, out CadBlockEntity modelBlock))
                {
                    Console.Error.WriteLine("Model block entity not found.");
                    return;
                }

                // Create a new MTEXT entity
                CadMText mtext = new CadMText
                {
                    Text = "Hello, Aspose.CAD MTEXT!",
                    InsertionPoint = new Cad3DPoint(10.0, 20.0, 0.0),
                    InitialTextHeight = 2.5,
                    RotationAngleRad = 0.0,
                    ReferenceRectangleWidth = 50.0,
                    SoftOwner = modelLayout.BlockTableRecordHandle
                };

                // Add the MTEXT entity to the Model space block
                modelBlock.AddEntity(mtext);

                // Append the new entity to the global Entities collection
                cadImage.Entities = cadImage.Entities
                    .Concat(new CadEntityBase[] { mtext })
                    .ToArray();

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified CAD file preserving editable objects
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };
                cadImage.Save(outputPath, dxfOptions);

                Console.WriteLine($"MTEXT entity added and file saved as {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}