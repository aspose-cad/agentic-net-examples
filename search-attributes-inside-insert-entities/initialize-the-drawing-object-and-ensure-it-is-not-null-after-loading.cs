using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace CadInitializationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD drawing file (DWG/DXF). Adjust the path as needed.
                string drawingPath = "sample.dwg";

                // Verify that the file exists before attempting to load it.
                if (!File.Exists(drawingPath))
                {
                    Console.Error.WriteLine($"File not found: {drawingPath}");
                    return;
                }

                // Load the drawing using Aspose.CAD.Image.Load and cast to CadImage.
                using (Image image = Image.Load(drawingPath))
                {
                    CadImage cadImage = image as CadImage;

                    // Ensure the drawing object was loaded successfully.
                    if (cadImage == null)
                    {
                        throw new InvalidOperationException("Failed to load the CAD drawing.");
                    }

                    // At this point cadImage is a valid, non‑null drawing object.
                    int entityCount = cadImage.Entities.Count();
                    Console.WriteLine("CAD drawing loaded successfully. Entity count: " + entityCount);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}