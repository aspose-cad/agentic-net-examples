using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace CadLoadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the CAD file (DWG, DXF, etc.)
            string sourcePath = "sample.dwg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Error: The file \"{sourcePath}\" does not exist.");
                return;
            }

            try
            {
                // Load the CAD drawing and cast to CadImage
                using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
                {
                    // Verify that the image was loaded successfully
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load the CAD drawing.");
                        return;
                    }

                    // Optional: check that the drawing contains entities
                    int entityCount = cadImage.Entities != null ? cadImage.Entities.Count() : 0;
                    Console.WriteLine($"CAD drawing loaded successfully. Entity count: {entityCount}");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during loading
                Console.WriteLine($"An error occurred while loading the CAD drawing: {ex.Message}");
            }
        }
    }
}
