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
            try
            {
                // Path to the source DWG or DXF file
                string sourcePath = "sample.dwg";

                // Verify the file exists before attempting to load
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"File not found: {sourcePath}");
                    return;
                }

                // Load the drawing into a CadImage object
                CadImage cadImage = (CadImage)Image.Load(sourcePath);

                // Output basic information about the loaded drawing
                Console.WriteLine($"Loaded drawing: {sourcePath}");
                Console.WriteLine($"Number of entities: {cadImage.Entities?.Count() ?? 0}");
                Console.WriteLine($"Number of layouts: {cadImage.Layouts?.Count ?? 0}");
                Console.WriteLine($"Number of block definitions: {cadImage.BlockEntities?.Count ?? 0}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}