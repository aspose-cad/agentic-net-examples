using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the DWG file. Provide via command line or replace with a literal path.
                string dwgPath = args.Length > 0 ? args[0] : "sample.dwg";

                if (!File.Exists(dwgPath))
                {
                    Console.Error.WriteLine($"File not found: {dwgPath}");
                    return;
                }

                // Load the DWG file using Aspose.CAD.Image.Load and cast to CadImage.
                using (CadImage cadImage = (CadImage)Image.Load(dwgPath))
                {
                    Console.WriteLine($"Loaded DWG file: {dwgPath}");
                    Console.WriteLine("Listing entity types contained in the drawing:");

                    // Iterate over top‑level entities in the drawing.
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Output the concrete .NET type name of each entity.
                        Console.WriteLine($"- {entity.GetType().Name}");
                    }
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