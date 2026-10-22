using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace CadLayoutTextSuperimpose
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input CAD file and output directory
            string inputFilePath = @"C:\CAD\sample.dwg";
            string outputDirectory = @"C:\CAD\Output";
            string outputFilePath = Path.Combine(outputDirectory, "sample_modified.dxf");

            // Validate input file
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine("Input file does not exist: " + inputFilePath);
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            try
            {
                // Load the CAD image
                using (Image cadImage = Image.Load(inputFilePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cad = cadImage as CadImage;
                    if (cad == null)
                    {
                        Console.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Iterate through each layout in the CAD file
                    foreach (CadLayout layout in cad.Layouts.Values)
                    {
                        // Retrieve the block entity that represents the layout's block table record
                        if (!cad.BlockEntities.ContainsKey(layout.BlockTableRecordHandle))
                        {
                            Console.WriteLine($"Block entity not found for layout '{layout.LayoutName}'.");
                            continue;
                        }

                        CadBlockEntity blockEntity = cad.BlockEntities[layout.BlockTableRecordHandle] as CadBlockEntity;
                        if (blockEntity == null)
                        {
                            Console.WriteLine($"Block entity is not a CadBlockEntity for layout '{layout.LayoutName}'.");
                            continue;
                        }

                        // Create a new CadText entity
                        CadText textEntity = new CadText
                        {
                            DefaultValue = "Sample Text",
                            FirstAlignment = new Cad3DPoint(10.0, 10.0, 0.0),
                            TextHeight = 2.5,
                            TextRotation = 0.0,
                            SoftOwner = layout.BlockTableRecordHandle
                        };

                        // Add the text entity to the block (layout)
                        blockEntity.AddEntity(textEntity);
                    }

                    // Save the modified CAD file preserving editable objects
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cad.Save(outputFilePath, saveOptions);
                }

                Console.WriteLine("CAD file processed and saved to: " + outputFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}