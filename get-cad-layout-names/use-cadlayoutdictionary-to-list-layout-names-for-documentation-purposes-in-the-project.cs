using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace CadLayoutLister
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file (DWG, DXF, DWF, etc.)
                string sourcePath = "input.dwg"; // TODO: replace with actual file path

                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"File not found: {sourcePath}");
                    return;
                }

                // Load the CAD file
                using (var cadImage = (CadImage)Image.Load(sourcePath))
                {
                    // Retrieve the layout dictionary from the loaded image
                    var layoutDictionary = cadImage.Layouts; // IReadOnlyDictionary<string, CadLayout>

                    // List all layout names
                    Console.WriteLine("Available layouts in the CAD file:");
                    foreach (var layoutName in layoutDictionary.Keys)
                    {
                        Console.WriteLine("- " + layoutName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}