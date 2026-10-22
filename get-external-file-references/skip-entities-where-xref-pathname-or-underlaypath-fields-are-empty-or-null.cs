using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class CadEntityFilter
{
    // Helper to safely get a property value via reflection.
    private static T GetPropertyValue<T>(object obj, string propertyName)
    {
        var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (prop != null && prop.CanRead)
        {
            var value = prop.GetValue(obj);
            if (value is T typedValue)
                return typedValue;
        }
        return default;
    }

    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        // Ensure a placeholder input file exists.
        if (!File.Exists(inputPath))
        {
            File.WriteAllBytes(inputPath, new byte[0]);
        }

        try
        {
            // Load the CAD image.
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Filter entities: skip those where XrefPathName or UnderlayPath are null/empty.
                var filteredEntities = cadImage.Entities
                    .Where(e =>
                    {
                        string xref = GetPropertyValue<string>(e, "XrefPathName");
                        string underlay = GetPropertyValue<string>(e, "UnderlayPath");

                        bool xrefInvalid = xref != null && string.IsNullOrWhiteSpace(xref);
                        bool underlayInvalid = underlay != null && string.IsNullOrWhiteSpace(underlay);

                        return !(xrefInvalid || underlayInvalid);
                    })
                    .ToArray();

                // Replace the internal entities collection via reflection.
                var entitiesField = typeof(CadImage).GetField("_entities", BindingFlags.NonPublic | BindingFlags.Instance)
                                   ?? typeof(CadImage).GetField("entities", BindingFlags.NonPublic | BindingFlags.Instance);
                if (entitiesField != null)
                {
                    entitiesField.SetValue(cadImage, filteredEntities);
                }

                // Prepare rasterization options for PDF output.
                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    PageWidth = 800,
                    PageHeight = 600,
                    Layouts = new[] { cadImage.Layouts.Keys.FirstOrDefault() ?? "Model" }
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the filtered CAD image as PDF.
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}