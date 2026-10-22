using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path (DWG/DXF)
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Desired output PDF file path
            string outputPath = "output.pdf";

            try
            {
                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Configure rasterization options (e.g., page size)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    // Collect only visible (not off and not frozen) layer names
                    List<string> visibleLayers = new List<string>();
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        bool isOff = layer.ColorId < 0;
                        bool isFrozen = (layer.Flags & 1) != 0; // Flag bit 0 indicates frozen

                        if (!isOff && !isFrozen)
                        {
                            visibleLayers.Add(layer.Name);
                        }
                    }

                    // If there are visible layers, assign them; otherwise export all layers
                    if (visibleLayers.Count > 0)
                    {
                        rasterOptions.Layers = visibleLayers.ToArray();
                    }

                    // Set up PDF save options and attach the rasterization settings
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine("CAD file successfully converted to PDF.");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during loading or saving
                Console.WriteLine("Error during CAD conversion: " + ex.Message);
            }
        }
    }
}