using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadBlockEntityDeletion
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "input.dwg";
            const string outputPath = "output.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Iterate over all block definitions in the drawing
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        // Collect entities that meet the deletion criteria
                        List<CadEntityBase> entitiesToDelete = new List<CadEntityBase>();

                        // Examine each entity inside the current block
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            // Identify rectangle-like entities: lightweight polylines with 4 points and closed
                            if (entity is CadLwPolyline polyline &&
                                polyline.Coordinates != null &&
                                polyline.Coordinates.Count == 4 &&
                                (polyline.Flag & CadLwPolylineFlag.Closed) != 0)
                            {
                                // Compute bounding box of the polyline
                                double minX = double.MaxValue;
                                double minY = double.MaxValue;
                                double maxX = double.MinValue;
                                double maxY = double.MinValue;

                                foreach (Cad2DPoint pt in polyline.Coordinates)
                                {
                                    if (pt.X < minX) minX = pt.X;
                                    if (pt.Y < minY) minY = pt.Y;
                                    if (pt.X > maxX) maxX = pt.X;
                                    if (pt.Y > maxY) maxY = pt.Y;
                                }

                                double width = maxX - minX;
                                double height = maxY - minY;

                                // Example condition: delete rectangles larger than 100 units in either dimension
                                if (width > 100.0 || height > 100.0)
                                {
                                    entitiesToDelete.Add(polyline);
                                }
                            }
                        }

                        // Bulk removal of the collected entities from the block
                        foreach (CadEntityBase entity in entitiesToDelete)
                        {
                            block.Entities.Remove(entity);
                        }
                    }

                    // Save the modified drawing
                    cadImage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}