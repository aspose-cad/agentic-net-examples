using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Path to the source DWG file
        string sourcePath = "input.dwg";
        // Path to the output file (PDF in this example)
        string outputPath = "output.pdf";

        // Verify source file exists
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Ensure output directory exists
        string? outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the DWG file
            using (Image image = Image.Load(sourcePath))
            {
                // Cast to CadImage to work with CAD object model
                CadImage cadImage = (CadImage)image;

                // Modify the first line entity found (if any)
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLine line)
                    {
                        // Translate the line by +10 units on the X axis
                        line.FirstPoint = new Cad3DPoint(line.FirstPoint.X + 10, line.FirstPoint.Y, line.FirstPoint.Z);
                        line.SecondPoint = new Cad3DPoint(line.SecondPoint.X + 10, line.SecondPoint.Y, line.SecondPoint.Z);
                        // Change the line's layer name
                        line.LayerName = "ModifiedLayer";
                        // Stop after modifying the first line
                        break;
                    }
                }

                // Add a new line to the drawing
                CadLine newLine = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 0, 0),
                    LayerName = "NewLayer"
                };
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { newLine }).ToArray();

                // Set rasterization options for vector output
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1000,
                    PageHeight = 1000,
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f
                };

                // Save the modified drawing as PDF
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}