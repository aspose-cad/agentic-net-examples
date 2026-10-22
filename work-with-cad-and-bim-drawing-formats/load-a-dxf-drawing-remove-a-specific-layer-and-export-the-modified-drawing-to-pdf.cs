using System;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the source DXF file
                string inputPath = "input.dxf";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the DXF drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage to work with CAD-specific members
                    CadImage cad = (CadImage)cadImage;

                    // Name of the layer to be removed
                    string layerToRemove = "LayerToRemove";

                    // Filter out entities that belong to the specified layer
                    CadEntityBase[] filteredEntities = cad.Entities
                        .Where(entity => !string.Equals(GetLayerName(entity), layerToRemove, StringComparison.OrdinalIgnoreCase))
                        .ToArray();

                    // Replace the Entities collection with the filtered set
                    cad.Entities = filteredEntities;

                    // Save the modified drawing as PDF
                    string outputPath = "output.pdf";
                    PdfOptions pdfOptions = new PdfOptions();
                    cad.Save(outputPath, pdfOptions);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}

    // Retrieves the layer name of a CAD entity (uses reflection as a fallback)
    private static string GetLayerName(CadEntityBase entity)
    {
        var layerProperty = entity.GetType().GetProperty("LayerName");
        if (layerProperty != null)
        {
            object value = layerProperty.GetValue(entity);
            return value != null ? value.ToString() : string.Empty;
        }
        return string.Empty;
    }
}