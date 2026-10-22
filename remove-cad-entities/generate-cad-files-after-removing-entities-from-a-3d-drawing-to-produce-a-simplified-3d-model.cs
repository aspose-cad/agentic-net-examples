using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
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
            // Load the source CAD drawing (DWG/DXF)
            using (Image sourceImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)sourceImage;

                // Remove all 3D faces from the model by creating a new entity collection
                var filteredEntities = cadImage.Entities
                    .Where(e => !(e is Cad3DFace))
                    .ToArray();

                // Replace the existing entities with the filtered collection
                // (Entities property is assignable as an IEnumerable<CadEntityBase>)
                cadImage.Entities = filteredEntities;

                // Save the simplified drawing; format is inferred from the file extension
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}