using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace CadTableExtraction
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string sourcePath = "input.dxf";

                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                using (Image image = Image.Load(sourcePath))
                {
                    CadImage cadImage = (CadImage)image;

                    // The Aspose.CAD API does not expose DXF tables via CadImage.
                    // Therefore, extracting table data is not supported with the current SDK version.
                    Console.WriteLine("DXF table extraction is not available in the current Aspose.CAD API.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}