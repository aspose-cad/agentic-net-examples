using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadUnderlayChecker
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the source CAD file
                    string sourcePath = "input.dwg";

                    // Load the CAD file
                    using (Image image = Image.Load(sourcePath))
                    {
                        // Cast the loaded image to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Iterate through top-level CAD entities
                        foreach (CadEntityBase entity in cadImage.Entities)
                        {
                            // Check if the entity is a DGN underlay
                            if (entity is CadDgnUnderlay dgnUnderlay)
                            {
                                bool isValid = IsValidUnderlayPath(dgnUnderlay);
                                Console.WriteLine($"DGN Underlay Path \"{dgnUnderlay.UnderlayPath}\" is valid: {isValid}");
                            }
                        }
                    }
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}

        // Helper method that returns true when a CadDgnUnderlay entity has a valid UnderlayPath
        static bool IsValidUnderlayPath(CadDgnUnderlay underlay)
        {
            // Ensure the UnderlayPath property is not null or empty
            if (string.IsNullOrEmpty(underlay.UnderlayPath))
                return false;

            // Optionally, verify that the file exists on disk
            return File.Exists(underlay.UnderlayPath);
        }
    }
}