using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AsposeCadUnderlayReader
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder path to the CAD file (DWG/DXF etc.)
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"File not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Iterate over all top‑level entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Check for DGN underlay entities
                        if (entity is CadDgnUnderlay dgnUnderlay)
                        {
                            // Output the UnderlayPath field
                            Console.WriteLine($"Underlay Path: {dgnUnderlay.UnderlayPath}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}