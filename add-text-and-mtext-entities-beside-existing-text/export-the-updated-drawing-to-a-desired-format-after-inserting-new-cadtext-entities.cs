using System.Linq;
using System;
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
                // Path to the source DWG/DXF file
                string inputPath = "input.dwg";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path for the exported image
                string outputPath = "output.png";

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Create a new MText entity
                    CadMText mText = new CadMText();
                    mText.Text = "Hello Aspose.CAD";
                    mText.InsertionPoint = new Cad3DPoint(0, 0, 0);
                    mText.InitialTextHeight = 2.5;
                    mText.RotationAngleRad = 0;

                    // Add the MText entity to the drawing
                    cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { mText }).ToArray();

                    // Save the updated drawing as PNG
                    PngOptions pngOptions = new PngOptions();
                    cadImage.Save(outputPath, pngOptions);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}