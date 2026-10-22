using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace InsertDwGExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Paths for source (block to insert) and target DWG files
            string sourcePath = @"C:\CAD\source.dwg";
            string targetPath = @"C:\CAD\target.dwg";
            string outputPath = @"C:\CAD\output\merged.dwg";

            try
            {
                // Validate input files
                if (!File.Exists(sourcePath))
                    throw new FileNotFoundException("Source DWG not found.", sourcePath);
                if (!File.Exists(targetPath))
                    throw new FileNotFoundException("Target DWG not found.", targetPath);

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);

                // Load source and target DWG images
                CadImage sourceImage = (CadImage)Image.Load(sourcePath);
                CadImage targetImage = (CadImage)Image.Load(targetPath);

                // Retrieve the first block definition from the source drawing
                CadBlockEntity sourceBlock = sourceImage.BlockEntities.ValuesTyped.FirstOrDefault();
                if (sourceBlock == null)
                    throw new InvalidOperationException("Source DWG does not contain any block definitions.");

                // Create an insert object and configure its properties
                CadInsertObject insertObject = new CadInsertObject
                {
                    InsertionPoint = new Cad3DPoint(0.0, 0.0, 0.0),
                    Name = sourceBlock.Name, // reference the block to insert
                    ScaleX = 1.0,
                    ScaleY = 1.0,
                    ScaleZ = 1.0,
                    RotationAngle = 0.0
                };

                // Add the insert object to the Model Space block of the target drawing
                const string modelSpaceName = "*Model_Space";
                if (!targetImage.BlockEntities.ContainsKey(modelSpaceName))
                    throw new InvalidOperationException("Target DWG does not contain a Model Space block.");

                CadBlockEntity modelSpace = targetImage.BlockEntities[modelSpaceName];
                modelSpace.AddEntity(insertObject);

                // Save the modified target drawing preserving CAD entities
                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };

                targetImage.Save(outputPath, dxfOptions);

                Console.WriteLine("DWG drawing inserted and saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}