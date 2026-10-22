using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "EmptyDrawing.dxf");

            // Create an empty CAD image
            CadImage cadImage = new CadImage();

            // Save the newly created CAD file
            cadImage.Save(outputPath, new DxfOptions { OutputMode = CadOutputMode.Convert });

            Console.WriteLine($"CAD file created successfully at: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}