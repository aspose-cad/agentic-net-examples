using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadTitleBlockExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source DXF file
                string sourcePath = "input.dxf";
                // Path to the output DXF file
                string outputPath = "output_with_titleblock.dxf";

                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the DXF drawing
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // -------------------------------------------------
                    // Create a text entity for the title block
                    // -------------------------------------------------
                    var titleText = new CadText
                    {
                        DefaultValue = "Project Title",
                        FirstAlignment = new Cad3DPoint(0, 0, 0),
                        TextHeight = 10.0,
                        TextRotation = 0.0
                    };

                    // Find the Model Space block (default drawing space)
                    var modelSpaceBlock = cadImage.BlockEntities.ValuesTyped
                        .FirstOrDefault(b => string.Equals(b.Name, "*Model_Space", StringComparison.OrdinalIgnoreCase));

                    // If Model Space not found, use the first available block
                    if (modelSpaceBlock == null)
                    {
                        modelSpaceBlock = cadImage.BlockEntities.ValuesTyped.FirstOrDefault();
                    }

                    if (modelSpaceBlock != null)
                    {
                        // Insert the title text at the top of the layout (e.g., Y = 200)
                        titleText.FirstAlignment = new Cad3DPoint(0, 200, 0);
                        modelSpaceBlock.AddEntity(titleText);
                    }
                    else
                    {
                        Console.Error.WriteLine("No block space available to add the title text.");
                        return;
                    }

                    // -------------------------------------------------
                    // Save the modified drawing as DXF
                    // -------------------------------------------------
                    var saveOptions = new DxfOptions();
                    cadImage.Save(outputPath);
                }

                Console.WriteLine($"File saved successfully to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}