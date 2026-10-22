using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AsposeCadHyperlinkModification
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output paths
                string inputPath = "input.dwg";
                string outputDirectory = "Output";
                string outputPath = Path.Combine(outputDirectory, "modified.dxf");

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file '{inputPath}' not found. Please provide a valid CAD drawing.");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to cast loaded image to CadImage. The file may not be a supported CAD format.");
                        return;
                    }

                    // Iterate over top‑level entities and modify hyperlinks
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (!string.IsNullOrEmpty(entity.Hyperlink) && entity.Hyperlink.Contains("oldlink"))
                        {
                            entity.Hyperlink = entity.Hyperlink.Replace("oldlink", "newlink");
                            Console.WriteLine($"Modified hyperlink on entity of type {entity.GetType().Name}.");
                        }
                    }

                    // Also inspect entities inside block definitions
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (CadEntityBase blockEntity in block.Entities)
                        {
                            if (!string.IsNullOrEmpty(blockEntity.Hyperlink) && blockEntity.Hyperlink.Contains("oldlink"))
                            {
                                blockEntity.Hyperlink = blockEntity.Hyperlink.Replace("oldlink", "newlink");
                                Console.WriteLine($"Modified hyperlink in block '{block.Name}' on entity of type {blockEntity.GetType().Name}.");
                            }
                        }
                    }

                    // Prepare save options for DXF format (preserves editable objects)
                    var saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };

                    // Save the modified drawing
                    cadImage.Save(outputPath, saveOptions);
                    Console.WriteLine($"Modified drawing saved to '{outputPath}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}