using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the CAD file
            string cadFilePath = "sample.dwg";

            // Guard against missing file
            if (!File.Exists(cadFilePath))
            {
                Console.WriteLine($"File not found: {cadFilePath}");
                return;
            }

            // Load the CAD drawing
            using (Image image = Image.Load(cadFilePath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                // List to hold extracted text strings
                List<string> extractedTexts = new List<string>();

                // Iterate over top‑level entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Simple text entity
                    if (entity is CadText cadText)
                    {
                        extractedTexts.Add(cadText.DefaultValue);
                    }
                    // Multi‑line text entity
                    else if (entity is CadMText cadMText)
                    {
                        extractedTexts.Add(cadMText.Text);
                    }
                    // Inserted block reference – inspect its block definition
                    else if (entity is CadInsertObject cadInsert)
                    {
                        CadBlockEntity block = FindBlockByName(cadImage, cadInsert.Name);
                        if (block != null)
                        {
                            ExtractTextFromBlock(block, extractedTexts);
                        }
                    }
                }

                // Output the collected text
                Console.WriteLine("Extracted Text Content:");
                foreach (string txt in extractedTexts)
                {
                    Console.WriteLine(txt);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    // Finds a block definition by its name in the given CadImage
    private static CadBlockEntity FindBlockByName(CadImage cadImage, string blockName)
    {
        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
        {
            if (block.Name == blockName)
            {
                return block;
            }
        }
        return null;
    }

    // Recursively extracts text from entities inside a block definition
    private static void ExtractTextFromBlock(CadBlockEntity block, List<string> texts)
    {
        foreach (CadEntityBase entity in block.Entities)
        {
            if (entity is CadText cadText)
            {
                texts.Add(cadText.DefaultValue);
            }
            else if (entity is CadMText cadMText)
            {
                texts.Add(cadMText.Text);
            }
            else if (entity is CadInsertObject cadInsert)
            {
                // Nested block references can be handled similarly if needed.
                // For brevity, nested extraction is omitted.
            }
        }
    }
}