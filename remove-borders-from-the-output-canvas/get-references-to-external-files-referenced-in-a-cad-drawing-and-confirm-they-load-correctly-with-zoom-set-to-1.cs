using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadExternalReferenceChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the main CAD drawing
            string inputFilePath = "sample.dwg";

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Input file '{inputFilePath}' not found. Exiting.");
                return;
            }

            // Load the main CAD drawing
            using (Image mainImage = Image.Load(inputFilePath))
            {
                CadImage cadImage = mainImage as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage. Exiting.");
                    return;
                }

                // Collect external reference file paths
                List<string> externalReferences = new List<string>();

                // Iterate over block definitions
                foreach (CadBlockEntity blockEntity in cadImage.BlockEntities.ValuesTyped)
                {
                    // Iterate over entities inside each block
                    foreach (CadEntityBase entity in blockEntity.Entities)
                    {
                        // Look for insert objects which may reference external drawings
                        CadInsertObject insertObject = entity as CadInsertObject;
                        if (insertObject != null)
                        {
                            string referenceName = insertObject.Name;
                            if (!string.IsNullOrEmpty(referenceName) &&
                                (referenceName.EndsWith(".dwg", StringComparison.OrdinalIgnoreCase) ||
                                 referenceName.EndsWith(".dxf", StringComparison.OrdinalIgnoreCase) ||
                                 referenceName.EndsWith(".dgn", StringComparison.OrdinalIgnoreCase)))
                            {
                                // Resolve relative paths based on the main drawing's directory
                                string resolvedPath = Path.IsPathRooted(referenceName)
                                    ? referenceName
                                    : Path.Combine(Path.GetDirectoryName(inputFilePath), referenceName);
                                externalReferences.Add(resolvedPath);
                            }
                        }
                    }
                }

                // Prepare output directory for rendered external references
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "ExternalReferenceOutputs");
                Directory.CreateDirectory(outputDir);

                // Verify each external reference loads correctly with zoom set to 1
                foreach (string externalPath in externalReferences)
                {
                    if (!File.Exists(externalPath))
                    {
                        Console.WriteLine($"External reference not found: {externalPath}");
                        continue;
                    }

                    try
                    {
                        using (Image externalImage = Image.Load(externalPath))
                        {
                            // Render the external drawing to PNG with zoom = 1 to confirm loading
                            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                            {
                                Zoom = 1.0f,
                                PageWidth = 800,
                                PageHeight = 600,
                                DrawType = CadDrawTypeMode.UseObjectColor
                            };

                            PngOptions pngOptions = new PngOptions
                            {
                                VectorRasterizationOptions = rasterOptions
                            };

                            string outputFileName = Path.GetFileNameWithoutExtension(externalPath) + "_zoom1.png";
                            string outputPath = Path.Combine(outputDir, outputFileName);
                            externalImage.Save(outputPath, pngOptions);

                            Console.WriteLine($"Successfully loaded and rendered external reference: {externalPath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load external reference '{externalPath}': {ex.Message}");
                    }
                }

                if (externalReferences.Count == 0)
                {
                    Console.WriteLine("No external references found in the drawing.");
                }
            }
        }
    }
}