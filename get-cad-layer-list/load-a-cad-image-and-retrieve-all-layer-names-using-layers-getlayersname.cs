using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerNames
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the CAD file (replace with actual path)
            string cadFilePath = "sample.dwg";

            if (!File.Exists(cadFilePath))
            {
                Console.Error.WriteLine($"File not found: {cadFilePath}");
                return;
            }

            try
            {
                using (CadImage cadImage = (CadImage)Image.Load(cadFilePath))
                {
                    Console.WriteLine("Layer names in the drawing:");
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        Console.WriteLine(layer.Name);
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