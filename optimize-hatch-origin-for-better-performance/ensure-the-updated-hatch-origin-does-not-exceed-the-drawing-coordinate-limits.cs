using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace AsposeCadHatchOriginClamp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input and output file paths (replace with actual paths)
            string inputPath = @"C:\CAD\input.dxf";
            string outputPath = @"C:\CAD\output\clamped_output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            try
            {
                // Load the CAD drawing
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Determine drawing coordinate limits (bounding box)
                double minX = double.MaxValue;
                double maxX = double.MinValue;
                double minY = double.MaxValue;
                double maxY = double.MinValue;

                // Process entities in the main drawing
                foreach (var entity in cadImage.Entities)
                {
                    ProcessEntityForLimits(entity, ref minX, ref maxX, ref minY, ref maxY);
                }

                // Process entities inside block definitions
                foreach (var block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (var entity in block.Entities)
                    {
                        ProcessEntityForLimits(entity, ref minX, ref maxX, ref minY, ref maxY);
                    }
                }

                // NOTE: The current Aspose.CAD API does not expose a CadHatch class,
                // so hatch origin clamping cannot be performed here.
                // If a hatch class becomes available, similar logic to the one below
                // can be applied:
                //   var origin = hatch.Origin;
                //   double clampedX = Math.Max(minX, Math.Min(maxX, origin.X));
                //   double clampedY = Math.Max(minY, Math.Min(maxY, origin.Y));
                //   hatch.Origin = new Cad3DPoint(clampedX, clampedY, origin.Z);

                // Save the modified drawing
                var saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath, saveOptions);

                Console.WriteLine("Processing completed and file saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }

        // Helper method to update bounding limits based on an entity's points
        private static void ProcessEntityForLimits(CadEntityBase entity, ref double minX, ref double maxX, ref double minY, ref double maxY)
        {
            switch (entity)
            {
                case CadInsertObject insert:
                    UpdateLimits(insert.InsertionPoint, ref minX, ref maxX, ref minY, ref maxY);
                    break;
                case CadLine line:
                    UpdateLimits(line.FirstPoint, ref minX, ref maxX, ref minY, ref maxY);
                    UpdateLimits(line.SecondPoint, ref minX, ref maxX, ref minY, ref maxY);
                    break;
                case CadCircle circle:
                    UpdateLimits(circle.CenterPoint, ref minX, ref maxX, ref minY, ref maxY);
                    break;
                // Add other entity types as needed
            }
        }

        // Helper method to update bounding limits based on a point
        private static void UpdateLimits(Cad3DPoint point, ref double minX, ref double maxX, ref double minY, ref double maxY)
        {
            if (point == null) return;

            if (point.X < minX) minX = point.X;
            if (point.X > maxX) maxX = point.X;
            if (point.Y < minY) minY = point.Y;
            if (point.Y > maxY) maxY = point.Y;
        }
    }
}