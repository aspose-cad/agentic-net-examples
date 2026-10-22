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
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

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
                CadImage cadImage = (CadImage)image;

                // Text value of the entity to be removed
                string targetText = "RemoveMe";

                // Locate the TEXT entity in model space
                CadEntityBase entityToRemove = null;
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadText cadText && cadText.DefaultValue == targetText)
                    {
                        entityToRemove = cadText;
                        break;
                    }
                }

                // Remove the found entity if it exists
                if (entityToRemove != null)
                {
                    // CadImage.Entities is IEnumerable; recreate the collection without the target entity
                    var updatedEntities = cadImage.Entities.Where(e => e != entityToRemove).ToArray();

                    // Use reflection to set the read‑only Entities property
                    var entitiesProp = typeof(CadImage).GetProperty("Entities");
                    if (entitiesProp != null && entitiesProp.CanWrite)
                    {
                        entitiesProp.SetValue(cadImage, updatedEntities);
                    }
                    else
                    {
                        // Fallback: attempt to cast to a mutable collection if possible
                        if (cadImage.Entities is System.Collections.IList list)
                        {
                            list.Remove(entityToRemove);
                        }
                        else
                        {
                            Console.Error.WriteLine("Unable to remove entity: Entities collection is read‑only.");
                        }
                    }
                }

                // Recalculate drawing size after modification
                cadImage.UpdateSize();

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}