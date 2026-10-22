using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the CAD file (replace with your actual file path)
            string cadFilePath = "sample.dwg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(cadFilePath))
            {
                Console.WriteLine($"File not found: {cadFilePath}");
                return;
            }

            // Load the CAD file
            using (Image image = Image.Load(cadFilePath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Unable to load the CAD file as a CadImage.");
                    return;
                }

                // Retrieve layout names (keys of the Layouts dictionary)
                IList<string> layoutNames = new List<string>(cadImage.Layouts.Keys);

                Console.WriteLine("Available layouts:");
                foreach (string layoutName in layoutNames)
                {
                    Console.WriteLine("- " + layoutName);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}