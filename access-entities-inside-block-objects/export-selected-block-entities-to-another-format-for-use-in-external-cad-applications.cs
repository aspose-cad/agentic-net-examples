using System.Linq;
using System.Collections.Generic;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
                string inputPath = "sample.dxf";
                string outputPath = "output.pdf";

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                string outDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
                {
                    Directory.CreateDirectory(outDir);
                }

                // Load CAD image
                using (Image image = Image.Load(inputPath))
                {
                    if (image is CadImage cadImage)
                    {
                        // Find a block named "MyBlock"
                        CadBlockEntity targetBlock = null;
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            if (block.Name.Equals("MyBlock", StringComparison.OrdinalIgnoreCase))
                            {
                                targetBlock = block;
                                break;
                            }
                        }

                        if (targetBlock == null)
                        {
                            Console.WriteLine("Block 'MyBlock' not found.");
                            return;
                        }

                        // List entities inside the block
                        Console.WriteLine($"Entities inside block '{targetBlock.Name}':");
                        foreach (CadEntityBase entity in targetBlock.Entities)
                        {
                            Console.WriteLine($"- {entity.GetType().Name}");
                        }

                        // Export only existing inserts of the selected block.
                        var selected = new List<CadEntityBase>();
                        foreach (CadEntityBase entity in cadImage.Entities)
                        {
                            if (entity is CadInsertObject insert &&
                                string.Equals(insert.Name ?? insert.OriginalBlockName, targetBlock.Name, StringComparison.OrdinalIgnoreCase))
                                selected.Add(insert);
                        }
                        if (selected.Count == 0)
                            throw new InvalidOperationException("No instances of the selected block exist in this drawing.");
                        cadImage.Entities = selected.ToArray();
                        Console.WriteLine($"Exporting {selected.Count} insert(s) of block '{targetBlock.Name}'.");
                        // Export the selected block instances to PDF.
                        PdfOptions pdfOptions = new PdfOptions
                        {
                            VectorRasterizationOptions = new CadRasterizationOptions
                            {
                                PageWidth = 800,
                                PageHeight = 600,
                                DrawType = CadDrawTypeMode.UseObjectColor
                            }
                        };

                        cadImage.Save(outputPath, pdfOptions);
                        Console.WriteLine($"Exported to PDF: {outputPath}");
                    }
                    else
                    {
                        Console.WriteLine("Loaded file is not a CAD image.");
                    }
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}