using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace DetectOleObjects
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the DWG file (replace with actual path or pass as argument)
            string dwgPath = "input.dwg";

            // Guard against missing input file
            if (!File.Exists(dwgPath))
            {
                Console.Error.WriteLine($"Input file not found: {dwgPath}");
                return;
            }

            try
            {
                // Load the DWG drawing using Aspose.CAD
                using (Image image = Image.Load(dwgPath))
                {
                    // Cast the generic Image to CadImage to access CAD-specific members
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // NOTE: Aspose.CAD does not expose OLE object entities in its API.
                    // Therefore, detection and summarization of OLE objects cannot be performed.
                    Console.WriteLine("OLE object detection is not supported by Aspose.CAD.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}