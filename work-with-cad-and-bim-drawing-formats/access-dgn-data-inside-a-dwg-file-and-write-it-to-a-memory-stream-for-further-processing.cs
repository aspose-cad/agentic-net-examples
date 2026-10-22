using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the DWG file as a CadImage
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // NOTE: The Aspose.CAD library does not provide a DgnOptions class or direct
                // support for extracting embedded DGN data from a DWG file. Therefore, exporting
                // the DGN portion to a memory stream cannot be performed with the current SDK.

                Console.WriteLine("The Aspose.CAD SDK does not support extracting DGN data from a DWG file.");
                Console.WriteLine("Consider using a different library that provides DGN export capabilities.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}