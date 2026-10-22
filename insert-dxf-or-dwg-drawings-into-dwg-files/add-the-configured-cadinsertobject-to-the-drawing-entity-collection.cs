using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace CadInsertExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dxf";
            string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load an existing CAD drawing
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Create and configure a CadInsertObject
                CadInsertObject insertObject = new CadInsertObject
                {
                    InsertionPoint = new Cad3DPoint(10.0, 20.0, 0.0),
                    Name = "MyInsert",
                    // OriginalBlockName is read‑only; cannot be set directly.
                    ScaleX = 1.0,
                    ScaleY = 1.0,
                    ScaleZ = 1.0,
                    RotationAngle = 0.0
                };

                // Append the insert object to the drawing's entity collection
                var updatedEntities = cadImage.Entities
                    .Concat(new[] { insertObject })
                    .ToArray();

                // Replace the Entities collection
                cadImage.Entities = updatedEntities;

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified drawing preserving editable objects
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary,
                    Version = DxfOutputVersion.R12
                };

                cadImage.Save(outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}