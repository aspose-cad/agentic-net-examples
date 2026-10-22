using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace CadTableReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("The loaded file is not a CAD image.");
                        return;
                    }

                    // Aspose.CAD does not provide API for accessing table entities (rows/cells).
                    // Therefore, reading table rows and cells is not supported.
                    Console.WriteLine("Table extraction is not supported by the current Aspose.CAD SDK.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}