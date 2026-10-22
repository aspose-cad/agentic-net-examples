using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                // Layouts dictionary: key = layout name, value = layout object
                var layouts = cadImage.Layouts;

                // List all layout names
                foreach (var layoutName in layouts.Keys)
                {
                    Console.WriteLine($"Layout name: {layoutName}");
                }

                // Select the first layout (if any) for further processing
                var selectedLayoutName = layouts.Keys.FirstOrDefault();
                if (selectedLayoutName != null)
                {
                    var selectedLayout = layouts[selectedLayoutName];
                    Console.WriteLine($"Selected layout: {selectedLayoutName}");
                    // Additional processing of selectedLayout can be added here
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}