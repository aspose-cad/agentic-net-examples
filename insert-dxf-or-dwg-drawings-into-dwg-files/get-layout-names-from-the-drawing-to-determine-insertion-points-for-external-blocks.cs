using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadLayoutExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string drawingPath = "input.dwg";

                if (!File.Exists(drawingPath))
                {
                    Console.Error.WriteLine($"File not found: {drawingPath}");
                    return;
                }

                using (Image loadedImage = Image.Load(drawingPath))
                {
                    if (loadedImage is CadImage cadImage)
                    {
                        Console.WriteLine("Layouts found in the drawing:");
                        foreach (CadLayout layout in cadImage.Layouts.Values)
                        {
                            Console.WriteLine("- " + layout.LayoutName);
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("The loaded file is not a CAD image.");
                    }
                }

                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}