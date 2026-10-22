using System;
using System.IO;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

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
                var cadImage = (CadImage)image;

                // Iterate through all top‑level entities
                foreach (var entity in cadImage.Entities)
                {
                    // Identify hatch entities by type name (case‑insensitive)
                    if (entity.GetType().Name.IndexOf("Hatch", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Try to locate an 'Origin' property
                        PropertyInfo originProp = entity.GetType().GetProperty("Origin", BindingFlags.Public | BindingFlags.Instance);
                        if (originProp != null && originProp.CanWrite)
                        {
                            // Create a point instance (assumes a constructor with two doubles)
                            object pointInstance = null;
                            try
                            {
                                pointInstance = Activator.CreateInstance(originProp.PropertyType, 0.0, 0.0);
                            }
                            catch
                            {
                                // Fallback: try parameterless constructor and set X/Y if available
                                pointInstance = Activator.CreateInstance(originProp.PropertyType);
                                PropertyInfo xProp = originProp.PropertyType.GetProperty("X");
                                PropertyInfo yProp = originProp.PropertyType.GetProperty("Y");
                                if (xProp != null && xProp.CanWrite) xProp.SetValue(pointInstance, 0.0);
                                if (yProp != null && yProp.CanWrite) yProp.SetValue(pointInstance, 0.0);
                            }

                            if (pointInstance != null)
                            {
                                originProp.SetValue(entity, pointInstance);
                            }
                        }
                    }
                }

                // Save the modified drawing as PDF
                var pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}