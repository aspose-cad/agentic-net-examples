using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadBlockAttributeInspection
{
    // DTO to hold attribute details
    public class AttributeInfo
    {
        public Cad2DPoint Location { get; set; }
        public string PromptString { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the DWG/DXF file
                string inputPath = "sample.dwg";

                // Ensure a placeholder file exists if the input is missing
                if (!File.Exists(inputPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(inputPath) ?? ".");
                    File.WriteAllBytes(inputPath, new byte[0]); // minimal placeholder
                    Console.WriteLine($"Placeholder input file created at: {inputPath}");
                    Console.WriteLine("Cannot process a placeholder CAD file. Exiting.");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // List to store matched attribute details
                    List<AttributeInfo> matchedAttributes = new List<AttributeInfo>();

                    // Iterate over block definitions
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        // Iterate over entities inside the block
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            // The Aspose.CAD API does not expose a dedicated CadAttribute class
                            // or properties such as Position or PromptString. We can only identify
                            // an attribute by its TypeName, but cannot retrieve its details.
                            if (entity.TypeName.ToString().Equals("Attribute", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("Attribute entity detected, but Aspose.CAD does not provide access to its location or prompt string.");
                                // If needed, you could add a placeholder entry:
                                // matchedAttributes.Add(new AttributeInfo { Location = new Cad2DPoint(), PromptString = "N/A" });
                            }
                        }
                    }

                    // Output results
                    Console.WriteLine($"Found {matchedAttributes.Count} attribute(s) with retrievable details.");
                    foreach (AttributeInfo info in matchedAttributes)
                    {
                        Console.WriteLine($"Location: ({info.Location.X}, {info.Location.Y}), Prompt: {info.PromptString}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}