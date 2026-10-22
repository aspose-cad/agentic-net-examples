using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            // Layouts we want to render
            string[] requestedLayouts = new string[] { "Layout1", "Layout2" };

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load CAD file
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Check which requested layouts are missing
                List<string> missingLayouts = new List<string>();
                foreach (string layoutName in requestedLayouts)
                {
                    bool exists = false;
                    foreach (CadLayout layout in cadImage.Layouts.Values)
                    {
                        if (string.Equals(layout.LayoutName, layoutName, StringComparison.OrdinalIgnoreCase))
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                    {
                        missingLayouts.Add(layoutName);
                    }
                }

                // Report missing layouts and abort if any
                if (missingLayouts.Count > 0)
                {
                    Console.Error.WriteLine("The following requested layouts were not found in the CAD file:");
                    foreach (string missing in missingLayouts)
                    {
                        Console.Error.WriteLine($"- {missing}");
                    }
                    return;
                }

                // Set rasterization options with the requested layouts
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = requestedLayouts
                };

                // Prepare PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD file as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"File saved successfully to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}