using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.ImageOptions;

namespace CadMTextExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dxf";
            string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load an existing CAD file
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Get the first layout
                CadLayout layout = null;
                foreach (var l in cadImage.Layouts.Values)
                {
                    layout = l;
                    break;
                }

                if (layout == null)
                {
                    Console.WriteLine("No layout found in the CAD file.");
                    return;
                }

                // Retrieve the block entity that corresponds to the layout's block table record
                string blockHandle = layout.BlockTableRecordHandle;
                if (!cadImage.BlockEntities.ContainsKey(blockHandle))
                {
                    Console.WriteLine("Corresponding block entity not found.");
                    return;
                }

                CadBlockEntity blockEntity = cadImage.BlockEntities[blockHandle];

                // Insert multiple MTEXT entities using a loop
                for (int i = 0; i < 5; i++)
                {
                    CadMText mtext = new CadMText
                    {
                        Text = $"Sample MTEXT {i}",
                        InsertionPoint = new Cad3DPoint(i * 10.0, i * 5.0, 0.0),
                        InitialTextHeight = 2.5,
                        RotationAngleRad = 0.0,
                        ReferenceRectangleWidth = 50.0,
                        SoftOwner = blockHandle
                    };

                    blockEntity.AddEntity(mtext);
                }

                // Note: CadBlockEntity.UpdateSize is not available in the current Aspose.CAD API,
                // so size update cannot be performed programmatically.

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified CAD file preserving editable objects
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };

                cadImage.Save(outputPath);

                Console.WriteLine("MTEXT entities added and file saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}