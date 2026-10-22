using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string filePath = "sample.dwg";

        // Ensure the input file exists; create a minimal placeholder if it does not.
        if (!File.Exists(filePath))
        {
            // Create an empty placeholder file. In a real scenario, this should be a valid DWG/DXF file.
            File.WriteAllText(filePath, string.Empty);
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(filePath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Retrieve a PromptString value (using the first block's name as an example)
                string promptString = string.Empty;
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    promptString = block.Name;
                    break; // only need the first block for this example
                }

                // Desired search string
                string desiredSearch = "MyBlock";

                // Compare the retrieved PromptString with the desired search string (case‑insensitive)
                bool isMatch = string.Equals(promptString, desiredSearch, StringComparison.OrdinalIgnoreCase);

                // Output the result
                Console.WriteLine($"PromptString: '{promptString}'");
                Console.WriteLine($"Desired: '{desiredSearch}'");
                Console.WriteLine($"Match: {isMatch}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
    }
}