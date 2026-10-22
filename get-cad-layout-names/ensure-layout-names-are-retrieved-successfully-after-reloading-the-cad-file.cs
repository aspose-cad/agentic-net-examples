using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the CAD file (replace with actual file path)
            string cadFilePath = "input.dwg";

            if (!File.Exists(cadFilePath))
            {
                Console.Error.WriteLine($"File not found: {cadFilePath}");
                return;
            }

            // Load the CAD file for the first time
            using (Image cadImage = Image.Load(cadFilePath))
            {
                // Cast to CadImage to access layout information
                CadImage cad = (CadImage)cadImage;

                // Retrieve layout names before reload
                IList<string> layoutsBefore = cad.Layouts.Keys.ToList();

                Console.WriteLine("Layouts before reload:");
                foreach (string layoutName in layoutsBefore)
                {
                    Console.WriteLine(layoutName);
                }
            }

            // Reload the CAD file
            using (Image cadImageReloaded = Image.Load(cadFilePath))
            {
                // Cast to CadImage to access layout information
                CadImage cadReloaded = (CadImage)cadImageReloaded;

                // Retrieve layout names after reload
                IList<string> layoutsAfter = cadReloaded.Layouts.Keys.ToList();

                Console.WriteLine("Layouts after reload:");
                foreach (string layoutName in layoutsAfter)
                {
                    Console.WriteLine(layoutName);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}