using System;
using System.IO;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the source CAD file (replace with actual path)
            string sourcePath = "sample.dwg";

            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source CAD file not found: {sourcePath}");
                return;
            }

            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);

            using (Image cadImage = Image.Load(sourcePath))
            {
                // Low quality rendering
                var lowRasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600
                };
                SetEnumProperty(lowRasterOptions, "Quality", "Low");
                SetEnumProperty(lowRasterOptions, "DrawType", "UseObjectColor");

                var lowPngOptions = new PngOptions
                {
                    VectorRasterizationOptions = lowRasterOptions
                };
                string lowPath = Path.Combine(outputDir, "low_quality.png");
                cadImage.Save(lowPath, lowPngOptions);
                Console.WriteLine($"Low quality image saved to: {lowPath}");

                // High quality rendering
                var highRasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600
                };
                SetEnumProperty(highRasterOptions, "Quality", "High");
                SetEnumProperty(highRasterOptions, "DrawType", "UseObjectColor");

                var highPngOptions = new PngOptions
                {
                    VectorRasterizationOptions = highRasterOptions
                };
                string highPath = Path.Combine(outputDir, "high_quality.png");
                cadImage.Save(highPath, highPngOptions);
                Console.WriteLine($"High quality image saved to: {highPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    // Helper to set enum properties via reflection without direct enum type reference
    private static void SetEnumProperty(object target, string propertyName, string enumValueName)
    {
        PropertyInfo prop = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (prop != null && prop.PropertyType.IsEnum)
        {
            object enumValue = Enum.Parse(prop.PropertyType, enumValueName);
            prop.SetValue(target, enumValue);
        }
    }
}