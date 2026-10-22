using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace FontAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            // Paths (adjust as needed)
            string inputCadPath = "input.dwg";
            string sourceFontsFolder = "SourceFonts";
            string customFontsFolder = "CustomFonts";

            // Ensure the custom fonts folder exists
            Directory.CreateDirectory(customFontsFolder);

            try
            {
                // Verify input CAD file exists
                if (!File.Exists(inputCadPath))
                {
                    Console.Error.WriteLine($"Input CAD file not found: {inputCadPath}");
                    return;
                }

                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputCadPath))
                {
                    // System fonts folder
                    string systemFontsPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

                    // Iterate through all entities to find text objects
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        ProcessEntity(entity, systemFontsPath, sourceFontsFolder, customFontsFolder);
                    }

                    // Optionally, set a default font for the CAD image (if needed)
                    // cadImage.DefaultFont = "Arial";

                    // Save the CAD file if you want to keep changes (optional)
                    // cadImage.Save("output.dwg");
                }

                Console.WriteLine("Font check completed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Processes a CAD entity. For text entities, attempts to identify missing fonts.
        /// </summary>
        static void ProcessEntity(CadEntityBase entity, string systemFontsPath, string sourceFontsFolder, string customFontsFolder)
        {
            if (entity == null) return;

            // Handle simple text entities
            if (entity is CadText textEntity)
            {
                // The Aspose.CAD API does not expose a FontName property for CadText.
                // Therefore, font identification cannot be performed directly.
                Console.WriteLine("CadText entity encountered. Font name retrieval is not supported by the current API.");
                return;
            }

            // Handle multi-line text entities
            if (entity is CadMText mTextEntity)
            {
                // The Aspose.CAD API does not expose a FontName property for CadMText.
                // Therefore, font identification cannot be performed directly.
                Console.WriteLine("CadMText entity encountered. Font name retrieval is not supported by the current API.");
                return;
            }

            // Other entity types are ignored for font checking.
        }
    }
}