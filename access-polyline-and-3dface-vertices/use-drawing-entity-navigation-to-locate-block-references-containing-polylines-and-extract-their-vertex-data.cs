using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadBlockPolylineExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the DWG/DXF file
                string sourcePath = "input.dwg";

                // Guard against missing file
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"File not found: {sourcePath}");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Iterate over top‑level entities to find block insert objects
                    foreach (CadEntityBase topEntity in cadImage.Entities)
                    {
                        if (topEntity is CadInsertObject insertObject)
                        {
                            // Name of the block referenced by this insert
                            string blockName = insertObject.Name;

                            // Find the block definition with the same name by iterating the block dictionary
                            CadBlockEntity blockDefinition = null;
                            foreach (CadBlockEntity blk in cadImage.BlockEntities.ValuesTyped)
                            {
                                if (string.Equals(blk.Name, blockName, StringComparison.OrdinalIgnoreCase))
                                {
                                    blockDefinition = blk;
                                    break;
                                }
                            }

                            if (blockDefinition == null)
                            {
                                Console.WriteLine($"Block definition '{blockName}' not found.");
                                continue;
                            }

                            // Scan entities inside the block for lightweight polylines
                            foreach (CadEntityBase blockEntity in blockDefinition.Entities)
                            {
                                if (blockEntity is CadLwPolyline polyline)
                                {
                                    IList<Cad2DPoint> vertices = polyline.Coordinates;
                                    Console.WriteLine($"Polyline in block '{blockName}' (Insert at X:{insertObject.InsertionPoint.X}, Y:{insertObject.InsertionPoint.Y}):");
                                    for (int i = 0; i < vertices.Count; i++)
                                    {
                                        Cad2DPoint pt = vertices[i];
                                        Console.WriteLine($"  Vertex {i + 1}: X = {pt.X}, Y = {pt.Y}");
                                    }
                                }
                            }
                        }
                    }
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