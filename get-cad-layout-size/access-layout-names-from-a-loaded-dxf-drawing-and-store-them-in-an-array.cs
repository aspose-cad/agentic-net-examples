using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dxf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                var layoutTable = cadImage.Layouts;

                List<string> layoutNamesList = new List<string>();

                // Iterate through layout records and collect their names
                foreach (CadLayout layout in layoutTable.Values)
                {
                    layoutNamesList.Add(layout.LayoutName);
                }

                string[] layoutNames = layoutNamesList.ToArray();

                Console.WriteLine("Found layouts:");
                foreach (string name in layoutNames)
                {
                    Console.WriteLine(name);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}