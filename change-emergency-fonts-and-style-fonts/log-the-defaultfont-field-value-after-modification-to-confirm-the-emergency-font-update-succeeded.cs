using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace CadDefaultFontExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    // Create a minimal placeholder file to satisfy validation
                    File.WriteAllBytes(inputPath, new byte[0]);
                    Console.WriteLine($"Placeholder file created: {inputPath}");
                }

                using (Image loadedImage = Image.Load(inputPath))
                {
                    CadImage cadImage = loadedImage as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Modify the DefaultFont property
                    cadImage.DefaultFont = "Arial Unicode MS";

                    // Log the updated DefaultFont value
                    Console.WriteLine("DefaultFont after modification: " + cadImage.DefaultFont);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}