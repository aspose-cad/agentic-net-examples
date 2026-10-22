using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerLister
{
    class Program
    {
        static void Main(string[] args)
        {
            string cadFilePath = "sample.dwg";

            if (!File.Exists(cadFilePath))
            {
                Console.Error.WriteLine($"File not found: {cadFilePath}");
                return;
            }

            try
            {
                using (Image cadImageBase = Image.Load(cadFilePath))
                {
                    if (cadImageBase is CadImage cadImage)
                    {
                        foreach (CadLayerTable layer in cadImage.Layers)
                        {
                            Console.WriteLine(layer.Name);
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("Loaded image is not a CAD image.");
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