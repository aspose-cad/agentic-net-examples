using System;
using System.Collections;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            string outputPath = "output.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    if (image is not CadImage cadImage)
                    {
                        Console.Error.WriteLine("Loaded file is not a CAD image.");
                        return;
                    }

                    // Iterate block entities to locate external references (Xrefs)
                    foreach (DictionaryEntry entry in cadImage.BlockEntities)
                    {
                        if (entry.Value is CadBlockEntity blockEntity)
                        {
                            // Attempt to read a possible Path property that indicates an external reference
                            var pathProp = blockEntity.GetType().GetProperty("Path");
                            if (pathProp != null)
                            {
                                var pathValue = pathProp.GetValue(blockEntity) as string;
                                if (!string.IsNullOrEmpty(pathValue))
                                {
                                    var nameProp = blockEntity.GetType().GetProperty("Name");
                                    var nameValue = nameProp?.GetValue(blockEntity) as string;
                                    Console.WriteLine($"External reference found: Name = {nameValue}, Path = {pathValue}");
                                }
                            }
                        }
                    }

                    var rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = new[] { "Model" },
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"File saved successfully to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
