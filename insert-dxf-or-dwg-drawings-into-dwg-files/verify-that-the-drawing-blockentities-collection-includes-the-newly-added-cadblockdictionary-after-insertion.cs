using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg"; // Placeholder path to the DWG file
        const string newBlockName = "MyBlock"; // Name of the block to verify

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Verify that the BlockEntities collection contains the newly added block
                bool blockFound = false;
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (block.Name.Equals(newBlockName, StringComparison.OrdinalIgnoreCase))
                    {
                        blockFound = true;
                        break;
                    }
                }

                // Output verification result
                Console.WriteLine(blockFound
                    ? $"Block '{newBlockName}' is present in BlockEntities."
                    : $"Block '{newBlockName}' was not found in BlockEntities.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}