using System;
using System.Collections;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Placeholder path to the CAD drawing
        string inputPath = "input.dwg";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("The loaded file is not a supported CAD format.");
                    return;
                }

                Console.WriteLine("External file references found in the drawing:");

                bool anyReference = false;

                // Iterate through block entities and try to locate external references (Xrefs)
                foreach (DictionaryEntry entry in cadImage.BlockEntities)
                {
                    var block = entry.Value as CadBlockEntity;
                    if (block == null)
                        continue;

                    // Heuristic: external reference blocks usually have a type name containing "External"
                    if (block.GetType().Name.IndexOf("External", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        anyReference = true;

                        // Try to read a Path property if it exists
                        var pathProp = block.GetType().GetProperty("Path");
                        if (pathProp != null)
                        {
                            var pathValue = pathProp.GetValue(block) as string;
                            Console.WriteLine($"- {pathValue}");
                        }
                        else
                        {
                            // Fallback to block name
                            Console.WriteLine($"- {block.Name}");
                        }
                    }
                }

                if (!anyReference)
                {
                    Console.WriteLine("No external references were found.");
                }
            }
        }
        catch (Exception ex)
        {
            // Print concise error information
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}