using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadBlockExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string sourcePath = "source.dwg";
                string exportPath = "modular.dwg";

                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                using (Image sourceImage = Image.Load(sourcePath))
                {
                    CadImage sourceCad = (CadImage)sourceImage;

                    using (CadImage exportCad = new CadImage())
                    {
                        foreach (CadBlockEntity block in sourceCad.BlockEntities.ValuesTyped)
                        {
                            if (!string.IsNullOrEmpty(block.Name) &&
                                block.Name.StartsWith("Mod", StringComparison.OrdinalIgnoreCase))
                            {
                                exportCad.BlockEntities.Add(block.Name, block);
                            }
                        }

                        exportCad.Save(exportPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}