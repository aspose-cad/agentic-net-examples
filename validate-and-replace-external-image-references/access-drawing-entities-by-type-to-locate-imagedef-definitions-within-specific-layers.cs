using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dwg";
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            string targetLayerName = "ImagesLayer";

            var imageDefObjects = cadImage.Objects
                .OfType<CadRasterImageDef>()
                .Where(def => IsDefinitionUsedInLayer(def, cadImage, targetLayerName))
                .ToList();

            foreach (var def in imageDefObjects)
            {
                Console.WriteLine($"Found IMAGEDEF: Handle = {def.ObjectHandle}, FileName = {def.FileName}");
            }

            var rasterImages = cadImage.Entities
                .OfType<CadRasterImage>()
                .Where(img => imageDefObjects.Any(def => def.ObjectHandle == img.ImageDefReference))
                .ToList();

            foreach (var img in rasterImages)
            {
                Console.WriteLine($"Raster Image: InsertionPoint = ({img.InsertionPoint.X}, {img.InsertionPoint.Y}, {img.InsertionPoint.Z}), DefinitionHandle = {img.ImageDefReference}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    static bool IsDefinitionUsedInLayer(CadRasterImageDef def, CadImage cadImage, string layerName)
    {
        var referencingEntity = cadImage.Entities
            .OfType<CadRasterImage>()
            .FirstOrDefault(img => img.ImageDefReference == def.ObjectHandle);

        if (referencingEntity == null)
            return false;

        return string.Equals(referencingEntity.LayerName, layerName, StringComparison.OrdinalIgnoreCase);
    }
}