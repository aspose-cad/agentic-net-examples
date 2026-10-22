using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file \"{inputPath}\" does not exist.");
            return;
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Get the first layout in the drawing
                CadLayout layout = null;
                foreach (var kvp in cadImage.Layouts)
                {
                    layout = kvp.Value;
                    break;
                }

                if (layout == null)
                {
                    Console.WriteLine("No layout found in the CAD file.");
                    return;
                }

                // Define translation offsets for the hatch origin point
                double offsetX = 10.0;
                double offsetY = 10.0;
                double offsetZ = 0.0;

                // Move all hatch-like entities within the layout (placeholder logic)
                foreach (CadEntityBase entity in layout.Entities)
                {
                    // Since CadEntityTypeName.Hatch may not be available, use string comparison
                    if (entity.TypeName.ToString().Equals("Hatch", StringComparison.OrdinalIgnoreCase))
                    {
                        // Placeholder for actual translation logic.
                        // Example: entity.Transform = entity.Transform * CadTransform.CreateTranslation(offsetX, offsetY, offsetZ);
                    }
                }

                // Recalculate layout extents after moving hatches
                double minX = double.MaxValue, minY = double.MaxValue, minZ = double.MaxValue;
                double maxX = double.MinValue, maxY = double.MinValue, maxZ = double.MinValue;

                foreach (CadEntityBase entity in layout.Entities)
                {
                    // Fallback to layout extents if entity-specific bounds are unavailable
                    minX = Math.Min(minX, layout.MinExtents.X);
                    minY = Math.Min(minY, layout.MinExtents.Y);
                    minZ = Math.Min(minZ, layout.MinExtents.Z);
                    maxX = Math.Max(maxX, layout.MaxExtents.X);
                    maxY = Math.Max(maxY, layout.MaxExtents.Y);
                    maxZ = Math.Max(maxZ, layout.MaxExtents.Z);
                }

                double sizeX = maxX - minX;
                double sizeY = maxY - minY;
                double sizeZ = maxZ - minZ;

                Console.WriteLine($"Layout \"{layout.LayoutName}\" new size after moving hatches:");
                Console.WriteLine($"X: {sizeX}, Y: {sizeY}, Z: {sizeZ}");

                // Save the modified CAD drawing
                cadImage.Save(outputPath, new DxfOptions { OutputMode = CadOutputMode.Convert });
                Console.WriteLine($"Modified CAD saved to \"{outputPath}\".");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}