using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace AsposeCadFontValidation
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the custom font folder
                string customFontFolderPath = @"C:\CustomFonts";

                // Validate that the font folder exists
                if (!Directory.Exists(customFontFolderPath))
                {
                    Console.Error.WriteLine($"Font folder not found: {customFontFolderPath}");
                    return;
                }

                // Path to the CAD drawing to load
                string inputCadPath = @"C:\Input\drawing.dxf";

                // Validate that the CAD file exists
                if (!File.Exists(inputCadPath))
                {
                    Console.Error.WriteLine($"CAD file not found: {inputCadPath}");
                    return;
                }

                // Load the CAD drawing
                using (CadImage cadImage = (CadImage)Image.Load(inputCadPath))
                {
                    // Assign the custom font folder to the CadImage's DefaultFont property
                    cadImage.DefaultFont = customFontFolderPath;

                    // Example operation: output the number of entities in the drawing
                    int entityCount = cadImage.Entities?.Count() ?? 0;
                    Console.WriteLine($"Loaded CAD drawing with {entityCount} entities.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}