using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string sourcePath = "input.dwg";
            string outputPath = "output.dxf";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (Image cadImage = Image.Load(sourcePath))
            {
                CadImage cad = cadImage as CadImage;
                if (cad == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                if (cad.BlockEntities.ContainsKey("Model"))
                {
                    CadBlockEntity modelBlock = cad.BlockEntities["Model"];

                    var filtered = modelBlock.Entities
                                            .Where(e => !(e is CadLine))
                                            .ToList();

                    modelBlock.Entities.Clear();
                    foreach (var entity in filtered)
                    {
                        modelBlock.AddEntity(entity);
                    }
                }

                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };

                using (FileStream outStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    cadImage.Save(outStream);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}