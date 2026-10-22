using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadMTextExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD file
                string inputPath = "input.dxf";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD drawing as a CadImage
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Iterate through each layout in the drawing
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    // Collect all CadMText entities from the current layout
                    IEnumerable<CadMText> mTextEntities = layout.Entities
                        .OfType<CadMText>();

                    // Process each CadMText entity
                    foreach (CadMText mText in mTextEntities)
                    {
                        Console.WriteLine($"Layout: {layout.LayoutName}");
                        Console.WriteLine("MText Content:");
                        Console.WriteLine(mText.Text);
                        Console.WriteLine($"Insertion Point: X={mText.InsertionPoint.X}, Y={mText.InsertionPoint.Y}, Z={mText.InsertionPoint.Z}");
                        Console.WriteLine(new string('-', 40));
                    }
                }

                Console.WriteLine("Extraction completed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}