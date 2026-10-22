using System;
using System.Collections;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dwg";
        string layoutName = "Layout1";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Locate the block that represents the requested layout
                CadBlockEntity block = null;
                foreach (DictionaryEntry entry in cadImage.BlockEntities)
                {
                    if (entry.Key is string key && key.Equals(layoutName, StringComparison.OrdinalIgnoreCase))
                    {
                        block = entry.Value as CadBlockEntity;
                        break;
                    }
                }

                if (block == null)
                {
                    Console.WriteLine($"Layout '{layoutName}' not found.");
                    return;
                }

                // Compute extents of the layout
                double minX = double.MaxValue, minY = double.MaxValue, minZ = double.MaxValue;
                double maxX = double.MinValue, maxY = double.MinValue, maxZ = double.MinValue;

                foreach (CadEntityBase entity in block.Entities)
                {
                    UpdateExtents(entity, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                }

                double width = maxX - minX;
                double height = maxY - minY;
                double depth = maxZ - minZ;

                Console.WriteLine($"Layout '{layoutName}' size: Width={width}, Height={height}, Depth={depth}");

                // Rasterization options (fit layout into default page size)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = new[] { layoutName },
                    AutomaticLayoutsScaling = true,
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f,
                    PageWidth = 210.0f,   // A4 width in mm
                    PageHeight = 297.0f,  // A4 height in mm,
                    Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
                };

                // Render layout to PNG
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                string pngPath = "output.png";
                cadImage.Save(pngPath, pngOptions);
                Console.WriteLine($"Rendered layout saved to {pngPath}");

                // Extract vertex coordinates from Polyline and 3DFace entities within the layout
                foreach (CadEntityBase entity in block.Entities)
                {
                    if (entity is CadPolyline polyline)
                    {
                        Console.WriteLine("Polyline vertices:");
                        foreach (var pt in polyline.ChildObjects.OfType<Cad3DPoint>())
                        {
                            Console.WriteLine($"({pt.X}, {pt.Y}, {pt.Z})");
                        }
                    }
                    else if (entity is Cad3DFace face3d)
                    {
                        Console.WriteLine("3DFace vertices:");
                        PrintPoint(face3d.FirstCorner);
                        PrintPoint(face3d.SecondCorner);
                        PrintPoint(face3d.ThirdCorner);
                        PrintPoint(face3d.FourthCorner);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }

    static void UpdateExtents(CadEntityBase entity, ref double minX, ref double minY, ref double minZ,
                              ref double maxX, ref double maxY, ref double maxZ)
    {
        switch (entity)
        {
            case CadLine line:
                UpdatePoint(line.FirstPoint, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                UpdatePoint(line.SecondPoint, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                break;
            case CadPolyline polyline:
                foreach (var pt in polyline.ChildObjects.OfType<Cad3DPoint>())
                {
                    UpdatePoint(pt, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                }
                break;
            case Cad3DFace face3d:
                UpdatePoint(face3d.FirstCorner, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                UpdatePoint(face3d.SecondCorner, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                UpdatePoint(face3d.ThirdCorner, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                UpdatePoint(face3d.FourthCorner, ref minX, ref minY, ref minZ, ref maxX, ref maxY, ref maxZ);
                break;
            // Add handling for other entity types if needed
        }
    }

    static void UpdatePoint(Cad3DPoint point, ref double minX, ref double minY, ref double minZ,
                            ref double maxX, ref double maxY, ref double maxZ)
    {
        if (point.X < minX) minX = point.X;
        if (point.Y < minY) minY = point.Y;
        if (point.Z < minZ) minZ = point.Z;
        if (point.X > maxX) maxX = point.X;
        if (point.Y > maxY) maxY = point.Y;
        if (point.Z > maxZ) maxZ = point.Z;
    }

    static void PrintPoint(Cad3DPoint pt)
    {
        Console.WriteLine($"({pt.X}, {pt.Y}, {pt.Z})");
    }
}
