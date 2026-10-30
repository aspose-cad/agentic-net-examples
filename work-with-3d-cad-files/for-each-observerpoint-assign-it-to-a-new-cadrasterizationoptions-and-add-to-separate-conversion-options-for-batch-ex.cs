using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;

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

            var exportPoints = new List<ExportPoint>
            {
                new ExportPoint
                {
                    Name = "TopView",
                    OutputPath = "TopView.pdf",
                    Layouts = new[] { "Model" },
                    ObserverAngles = new[] { 0f, 0f, 0f }
                },
                new ExportPoint
                {
                    Name = "SideView",
                    OutputPath = "SideView.pdf",
                    Layouts = new[] { "Layout1" },
                    ObserverAngles = new[] { 0f, 90f, 0f }
                }
            };

            Image cadImage = Image.Load(inputPath);

            foreach (var point in exportPoints)
            {
                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(point.OutputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Create rasterization options specific to this observer point
                var cadOptions = new CadRasterizationOptions
                {
                    Layouts = point.Layouts,
                    ObserverPoint = new ObserverPoint(point.ObserverAngles[0], point.ObserverAngles[1], point.ObserverAngles[2])
                };

                // Set up PDF export options and assign the rasterization options
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = cadOptions
                };

                // Export the CAD image using the configured options
                cadImage.Save(point.OutputPath, pdfOptions);
                Console.WriteLine($"Exported {point.Name} to {point.OutputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

// Simple class representing an export configuration for an observer point
class ExportPoint
{
    public string Name { get; set; }
    public string OutputPath { get; set; }
    public string[] Layouts { get; set; }
    public float[] ObserverAngles { get; set; }
}