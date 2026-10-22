using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.ImageOptions;

namespace CadAnnotationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path
            string inputPath = "input.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load existing CAD file
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Iterate through each layout (layer)
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    // Check if the layout already contains any TEXT or MTEXT entities
                    bool hasText = layout.Entities.Any(e =>
                        e.TypeName == CadEntityTypeName.TEXT ||
                        e.TypeName == CadEntityTypeName.MTEXT);

                    if (hasText)
                    {
                        // Skip adding annotation to this layout to avoid duplication
                        continue;
                    }

                    // Create a new CadText annotation
                    CadText newText = new CadText
                    {
                        DefaultValue = "Sample Annotation",
                        FirstAlignment = new Cad3DPoint(10.0, 20.0, 0.0),
                        TextHeight = 2.5,
                        TextRotation = 0.0,
                        SoftOwner = layout.BlockTableRecordHandle
                    };

                    // Retrieve the block entity associated with the layout
                    CadBlockEntity blockEntity = cadImage.BlockEntities[layout.BlockTableRecordHandle];

                    // Add the new text entity to the block
                    blockEntity.AddEntity(newText);
                }

                // Prepare output path
                string outputPath = "output.dxf";
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified CAD file preserving editable objects
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Ascii,
                    Version = DxfOutputVersion.R12
                };

                cadImage.Save(outputPath);
                Console.WriteLine($"File saved successfully to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}