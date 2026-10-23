using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputDir = "Output";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Source file not found: {inputPath}");
                return;
            }

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded file is not a CAD image.");
                    return;
                }

                // Get the model space block where new entities can be added
                CadBlockEntity modelSpace = cadImage.BlockEntities.ValuesTyped
                    .FirstOrDefault(b => b.Name.Equals("*Model_Space", StringComparison.OrdinalIgnoreCase));

                if (modelSpace == null)
                {
                    Console.Error.WriteLine("Model space block not found.");
                    return;
                }

                // Enumerate all LwPolylines in the model space
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLwPolyline polyline)
                    {
                        // Determine if the polyline is closed using its Flag
                        bool isClosed = (polyline.Flag & CadLwPolylineFlag.Closed) == CadLwPolylineFlag.Closed;

                        if (isClosed)
                        {
                            Console.WriteLine($"Found closed polyline with {polyline.Coordinates?.Count ?? 0} vertices.");

                            // Create a hatch
                            CadHatch hatch = new CadHatch
                            {
                                PatternName = "SOLID",
                                HatchAngle = 0.0,
                                HatchScaleOrSpacing = 1.0,
                                AssociativeFlag = 0,
                                // Initialize required collections
                                BoundaryPaths = new List<CadHatchBoundaryPathContainer>(),
                                PatternDefinitions = new List<CadHatchPatternData>()
                            };

                            // Create an edge boundary path (placeholder – populate as needed)
                            CadEdgeBoundaryPath edgePath = new CadEdgeBoundaryPath();

                            // Wrap the edge path in a container
                            CadHatchBoundaryPathContainer container = new CadHatchBoundaryPathContainer
                            {
                                BoundaryPath = new List<ICadBoundaryPath> { edgePath },
                                PathType = 2,
                                SourceBoundaryObjects = new List<string>(),
                                BoundaryObjectCount = 0
                            };

                            // Assign the container list to the hatch
                            hatch.BoundaryPaths.Add(container);

                            // Add the hatch to the model space
                            modelSpace.AddEntity(hatch);
                        }
                    }
                }

                // Export the CAD to PDF for visual verification
                string pdfPath = Path.Combine(outputDir, "Result.pdf");
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600,
                        Layouts = new[] { "Model" },
                        UnitType = UnitType.Centimenter,
                        LineScale = 1.0f
                    }
                };
                cadImage.Save(pdfPath, pdfOptions);
                Console.WriteLine($"Exported PDF saved to: {pdfPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}