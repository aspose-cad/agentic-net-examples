using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadLayoutLayerSize
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the CAD file (DWG/DXF)
            string cadFilePath = "sample.dwg";

            // Verify file existence
            if (!File.Exists(cadFilePath))
            {
                Console.WriteLine($"File not found: {cadFilePath}");
                return;
            }

            try
            {
                // Load the CAD file
                using (Image image = Image.Load(cadFilePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Dictionary to hold bounding box per layer
                    var layerBounds = new Dictionary<string, (double MinX, double MaxX, double MinY, double MaxY, double MinZ, double MaxZ)>(StringComparer.OrdinalIgnoreCase);

                    // Helper to ensure a layer entry exists
                    void EnsureLayer(string layerName)
                    {
                        if (!layerBounds.ContainsKey(layerName))
                        {
                            layerBounds[layerName] = (double.MaxValue, double.MinValue, double.MaxValue, double.MinValue, double.MaxValue, double.MinValue);
                        }
                    }

                    // Process top‑level entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        string layerName = entity.LayerName ?? "Default";
                        EnsureLayer(layerName);
                        var bounds = layerBounds[layerName];

                        switch (entity)
                        {
                            case CadLine line:
                                // First point
                                bounds.MinX = Math.Min(bounds.MinX, line.FirstPoint.X);
                                bounds.MaxX = Math.Max(bounds.MaxX, line.FirstPoint.X);
                                bounds.MinY = Math.Min(bounds.MinY, line.FirstPoint.Y);
                                bounds.MaxY = Math.Max(bounds.MaxY, line.FirstPoint.Y);
                                bounds.MinZ = Math.Min(bounds.MinZ, line.FirstPoint.Z);
                                bounds.MaxZ = Math.Max(bounds.MaxZ, line.FirstPoint.Z);
                                // Second point
                                bounds.MinX = Math.Min(bounds.MinX, line.SecondPoint.X);
                                bounds.MaxX = Math.Max(bounds.MaxX, line.SecondPoint.X);
                                bounds.MinY = Math.Min(bounds.MinY, line.SecondPoint.Y);
                                bounds.MaxY = Math.Max(bounds.MaxY, line.SecondPoint.Y);
                                bounds.MinZ = Math.Min(bounds.MinZ, line.SecondPoint.Z);
                                bounds.MaxZ = Math.Max(bounds.MaxZ, line.SecondPoint.Z);
                                break;

                            case CadLwPolyline polyline:
                                foreach (var pt in polyline.Coordinates)
                                {
                                    bounds.MinX = Math.Min(bounds.MinX, pt.X);
                                    bounds.MaxX = Math.Max(bounds.MaxX, pt.X);
                                    bounds.MinY = Math.Min(bounds.MinY, pt.Y);
                                    bounds.MaxY = Math.Max(bounds.MaxY, pt.Y);
                                    // 2D polyline has no Z; treat Z as 0
                                    bounds.MinZ = Math.Min(bounds.MinZ, 0);
                                    bounds.MaxZ = Math.Max(bounds.MaxZ, 0);
                                }
                                break;

                            case Cad3DFace face:
                                // First corner
                                bounds.MinX = Math.Min(bounds.MinX, face.FirstCorner.X);
                                bounds.MaxX = Math.Max(bounds.MaxX, face.FirstCorner.X);
                                bounds.MinY = Math.Min(bounds.MinY, face.FirstCorner.Y);
                                bounds.MaxY = Math.Max(bounds.MaxY, face.FirstCorner.Y);
                                bounds.MinZ = Math.Min(bounds.MinZ, face.FirstCorner.Z);
                                bounds.MaxZ = Math.Max(bounds.MaxZ, face.FirstCorner.Z);
                                // Second corner
                                bounds.MinX = Math.Min(bounds.MinX, face.SecondCorner.X);
                                bounds.MaxX = Math.Max(bounds.MaxX, face.SecondCorner.X);
                                bounds.MinY = Math.Min(bounds.MinY, face.SecondCorner.Y);
                                bounds.MaxY = Math.Max(bounds.MaxY, face.SecondCorner.Y);
                                bounds.MinZ = Math.Min(bounds.MinZ, face.SecondCorner.Z);
                                bounds.MaxZ = Math.Max(bounds.MaxZ, face.SecondCorner.Z);
                                // Third corner
                                bounds.MinX = Math.Min(bounds.MinX, face.ThirdCorner.X);
                                bounds.MaxX = Math.Max(bounds.MaxX, face.ThirdCorner.X);
                                bounds.MinY = Math.Min(bounds.MinY, face.ThirdCorner.Y);
                                bounds.MaxY = Math.Max(bounds.MaxY, face.ThirdCorner.Y);
                                bounds.MinZ = Math.Min(bounds.MinZ, face.ThirdCorner.Z);
                                bounds.MaxZ = Math.Max(bounds.MaxZ, face.ThirdCorner.Z);
                                // Fourth corner (optional)
                                if (face.FourthCorner != null)
                                {
                                    bounds.MinX = Math.Min(bounds.MinX, face.FourthCorner.X);
                                    bounds.MaxX = Math.Max(bounds.MaxX, face.FourthCorner.X);
                                    bounds.MinY = Math.Min(bounds.MinY, face.FourthCorner.Y);
                                    bounds.MaxY = Math.Max(bounds.MaxY, face.FourthCorner.Y);
                                    bounds.MinZ = Math.Min(bounds.MinZ, face.FourthCorner.Z);
                                    bounds.MaxZ = Math.Max(bounds.MaxZ, face.FourthCorner.Z);
                                }
                                break;

                            // Additional entity types can be handled similarly
                        }

                        // Store updated bounds
                        layerBounds[layerName] = bounds;
                    }

                    // Output layer sizes
                    Console.WriteLine("Layer bounding boxes (including 3D depth):");
                    foreach (var kvp in layerBounds)
                    {
                        string layer = kvp.Key;
                        double width = kvp.Value.MaxX - kvp.Value.MinX;
                        double height = kvp.Value.MaxY - kvp.Value.MinY;
                        double depth = kvp.Value.MaxZ - kvp.Value.MinZ;

                        Console.WriteLine($"Layer: {layer}");
                        Console.WriteLine($"  Width : {width:F3}");
                        Console.WriteLine($"  Height: {height:F3}");
                        Console.WriteLine($"  Depth : {depth:F3}");
                        Console.WriteLine($"  Extents X [{kvp.Value.MinX:F3} .. {kvp.Value.MaxX:F3}]");
                        Console.WriteLine($"  Extents Y [{kvp.Value.MinY:F3} .. {kvp.Value.MaxY:F3}]");
                        Console.WriteLine($"  Extents Z [{kvp.Value.MinZ:F3} .. {kvp.Value.MaxZ:F3}]");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}