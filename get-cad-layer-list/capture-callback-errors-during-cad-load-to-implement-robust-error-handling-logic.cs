using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        // Define input CAD file and output directory
        string inputPath = "input.dwg";
        string outputDirectory = "output";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure the output directory exists
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        try
        {
            // Load the CAD file with default load options
            using (CadImage cadImage = (CadImage)Image.Load(inputPath, new LoadOptions()))
            {
                // Collect distinct layer names from the CAD entities
                HashSet<string> layerNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (!string.IsNullOrEmpty(entity.LayerName))
                    {
                        layerNames.Add(entity.LayerName);
                    }
                }

                // Output the list of layers
                Console.WriteLine("Layers found in the CAD file:");
                foreach (string layer in layerNames)
                {
                    Console.WriteLine($"- {layer}");
                }

                // Example: Save the CAD image as PDF
                string pdfPath = Path.Combine(outputDirectory, "output.pdf");
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(pdfPath, pdfOptions);
                Console.WriteLine($"PDF saved to: {pdfPath}");
            }
        }
        catch (Exception ex)
        {
            // Capture any errors that occur during loading or processing
            Console.WriteLine($"Error loading CAD file: {ex.Message}");
        }
    }
}