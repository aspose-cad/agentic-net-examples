using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace CadArcExport
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define output path
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output.dwg");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new CAD image
            using (CadImage cadImage = new CadImage())
            {
                // Create an arc entity
                CadArc arc = new CadArc
                {
                    CenterPoint = new Cad3DPoint { X = 0.0, Y = 0.0, Z = 0.0 },
                    Radius = 50.0,
                    StartAngle = 0.0,
                    EndAngle = Math.PI // 180 degrees in radians
                };

                // Add the arc to the ModelSpace block
                CadBlockEntity modelSpace = cadImage.BlockEntities.ValuesTyped.FirstOrDefault();
                if (modelSpace != null)
                {
                    modelSpace.AddEntity(arc);
                }
                else
                {
                    CadBlockEntity newBlock = new CadBlockEntity
                    {
                        BasePoint = new Cad3DPoint()
                    };
                    newBlock.AddEntity(arc);
                    cadImage.BlockEntities.Add("ModelSpace", newBlock);
                }

                // Prepare DWG save options
                DwgOptions dwgOptions = new DwgOptions
                {
                    OutputMode = CadOutputMode.Convert
                };

                // Save the CAD image as DWG with error handling
                try
                {
                    cadImage.Save(outputPath, dwgOptions);
                    Console.WriteLine($"DWG file saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving DWG file: {ex.Message}");
                }
            }
        }
    }
}