using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.ImageOptions;

namespace CadTextReplacement
{
    class Program
    {
        static void Main()
        {
            string inputFilePath = "input.dxf";
            string outputFilePath = "output.dxf";

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            try
            {
                using (CadImage cadImage = (CadImage)Image.Load(inputFilePath))
                {
                    // Select the first layout.
                    CadLayout targetLayout = cadImage.Layouts.Values.First();

                    // Create replacement text.
                    CadText replacementText = new CadText
                    {
                        DefaultValue = "New Annotation",
                        FirstAlignment = new Cad3DPoint(100.0, 200.0, 0.0),
                        TextHeight = 2.5,
                        TextRotation = 0.0,
                        SoftOwner = targetLayout.BlockTableRecordHandle
                    };

                    // Add the new text to the layout's block entity.
                    // Note: Direct removal of existing text entities is not supported by the current SDK.
                    if (cadImage.BlockEntities.ContainsKey(targetLayout.BlockTableRecordHandle))
                    {
                        CadBlockEntity blockEntity = cadImage.BlockEntities[targetLayout.BlockTableRecordHandle];
                        blockEntity.AddEntity(replacementText);
                    }

                    // Prepare DXF save options with explicit file format.
                    DxfOptions dxfSaveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,
                        DxfFileFormat = CadFileFormat.Binary
                    };

                    // Ensure output directory exists.
                    string outputDir = Path.GetDirectoryName(outputFilePath);
                    if (!string.IsNullOrEmpty(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Save the modified drawing.
                    cadImage.Save(outputFilePath, dxfSaveOptions);
                }

                Console.WriteLine("CAD text replacement completed successfully.");
                Console.WriteLine($"Input file : {inputFilePath}");
                Console.WriteLine($"Output file: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}