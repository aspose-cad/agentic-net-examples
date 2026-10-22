using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            string inputFile = "sample.dwg";
            if (!File.Exists(inputFile))
            {
                Console.Error.WriteLine($"Input file not found: {inputFile}");
                return;
            }

            using (Image image = Image.Load(inputFile))
            {
                if (image is not CadImage cadImage)
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                    return;
                }

                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    double width = layout.MaxExtents.X - layout.MinExtents.X;
                    double height = layout.MaxExtents.Y - layout.MinExtents.Y;

                    Console.WriteLine($"Layout \"{layout.LayoutName}\": Width = {width}, Height = {height}");

                    int bufferSize = (int)(width * height * 4);
                    byte[] buffer = new byte[bufferSize];
                    // Processing using the allocated buffer can be performed here.
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}