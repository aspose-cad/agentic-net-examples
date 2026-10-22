using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace ExtractOleObjects
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder path to the DWG file
            string inputPath = "sample.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the generic Image to a CadImage
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("The loaded file is not a CAD image.");
                        return;
                    }

                    // Aspose.CAD does not expose a specific CadOleObject type or OLE extraction API.
                    // Therefore, extracting OLE objects and listing their types is not supported.
                    Console.WriteLine("Extraction of OLE objects from DWG files is not supported by Aspose.CAD.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the file: {ex.Message}");
            }
        }
    }
}