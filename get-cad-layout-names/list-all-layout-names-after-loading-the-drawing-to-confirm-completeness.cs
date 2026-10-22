using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace AsposeCadLayoutListing
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "sample.dwg"; // TODO: replace with actual drawing file path

            try
            {
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                    return;
                }

                using (Image image = Image.Load(inputPath))
                {
                    if (image is not CadImage cadImage)
                    {
                        Console.Error.WriteLine("Loaded file is not a CAD image.");
                        return;
                    }

                    Console.WriteLine("Available layout names:");
                    foreach (var kvp in cadImage.Layouts)
                    {
                        // The key of the dictionary represents the layout name
                        Console.WriteLine($"- {kvp.Key}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}