using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputPath = "output/output.png";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing as a CadImage
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Determine the SoftOwner (layout block record handle)
                var firstLayout = cadImage.Layouts.Values.FirstOrDefault();
                string softOwner = firstLayout?.BlockTableRecordHandle ?? string.Empty;

                // Create a new MText entity
                var mText = new CadMText
                {
                    Text = "Hello Aspose.CAD",
                    InitialTextHeight = 2.5,
                    InsertionPoint = new Cad3DPoint(10, 10, 0),
                    RotationAngleRad = 0,
                    SoftOwner = softOwner
                };

                // Add the MText entity to the drawing's top-level entities collection
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { mText }).ToArray();

                // Save the modified drawing to a PNG raster image
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        DrawType = CadDrawTypeMode.UseObjectColor,
                        Layouts = new[] { "Model" }
                    }
                };

                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}