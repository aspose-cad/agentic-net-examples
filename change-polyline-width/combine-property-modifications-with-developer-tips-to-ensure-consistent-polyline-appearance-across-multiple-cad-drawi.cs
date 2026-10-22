using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace CadPolylineModifier
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Folder containing CAD drawings (DWG or DXF)
                string inputFolder = @"C:\CadDrawings\Input";
                // Folder to save modified drawings
                string outputFolder = @"C:\CadDrawings\Output";

                // Validate input folder existence
                if (!Directory.Exists(inputFolder))
                {
                    Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                    return;
                }

                // Ensure output directory exists
                Directory.CreateDirectory(outputFolder);

                // Process each CAD file in the input folder
                var cadFiles = Directory.GetFiles(inputFolder, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(f => f.EndsWith(".dwg", StringComparison.OrdinalIgnoreCase) ||
                                f.EndsWith(".dxf", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                if (cadFiles.Length == 0)
                {
                    Console.WriteLine("No CAD files found to process.");
                    return;
                }

                foreach (string filePath in cadFiles)
                {
                    // Load the CAD drawing
                    using (Image image = Image.Load(filePath))
                    {
                        // Cast to CadImage to access CAD object model
                        CadImage cadImage = (CadImage)image;

                        // ---- Modify top‑level polylines ----
                        foreach (CadLwPolyline polyline in cadImage.Entities.OfType<CadLwPolyline>())
                        {
                            // Ensure the polyline is closed for consistent appearance
                            polyline.Flag = CadLwPolylineFlag.Closed;
                        }

                        // ---- Modify polylines inside block definitions ----
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            foreach (CadLwPolyline polyline in block.Entities.OfType<CadLwPolyline>())
                            {
                                // Ensure the polyline is closed inside the block as well
                                polyline.Flag = CadLwPolylineFlag.Closed;
                            }
                        }

                        // Save the modified drawing preserving editable objects
                        string outputPath = Path.Combine(outputFolder, Path.GetFileName(filePath));
                        DxfOptions saveOptions = new DxfOptions
                        {
                            OutputMode = CadOutputMode.Convert
                        };
                        cadImage.Save(outputPath, saveOptions);
                    }
                }

                Console.WriteLine("Processing completed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}