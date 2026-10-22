using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Path to the DWG file to be searched
        string inputPath = "sample.dwg";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the DWG file as a CadImage
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Text strings to search for
                string[] searchStrings = new[] { "Level 1", "Level 2" };

                // Iterate through all CAD entities
                foreach (var entity in cadImage.Entities)
                {
                    // Process CadText entities
                    if (entity is CadText textEntity)
                    {
                        string text = textEntity.DefaultValue;
                        if (string.IsNullOrEmpty(text))
                            continue;

                        foreach (string search in searchStrings)
                        {
                            if (text.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                Console.WriteLine($"Found \"{search}\" in CadText entity.");
                            }
                        }
                    }
                    // Process CadMText entities
                    else if (entity is CadMText mTextEntity)
                    {
                        string text = mTextEntity.Text;
                        if (string.IsNullOrEmpty(text))
                            continue;

                        foreach (string search in searchStrings)
                        {
                            if (text.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                Console.WriteLine($"Found \"{search}\" in CadMText entity.");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Print concise error information
            Console.Error.WriteLine($"Error processing DWG file: {ex.Message}");
        }
    }
}