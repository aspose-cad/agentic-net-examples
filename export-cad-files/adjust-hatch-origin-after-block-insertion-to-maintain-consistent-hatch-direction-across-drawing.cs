using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.dxf";
            string outputPath = "output/sample_modified.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
                Directory.CreateDirectory(outDir);

            using (var cadImage = (CadImage)Image.Load(inputPath))
            {
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadInsertObject insertObj)
                    {
                        Cad3DPoint insertionPt = insertObj.InsertionPoint;

                        if (cadImage.BlockEntities.TryGetValue(insertObj.Name, out CadBlockEntity block))
                        {
                            foreach (var blockEntity in block.Entities)
                            {
                                if (blockEntity is CadHatch hatch)
                                {
                                    if (hatch.PatternDefinitions != null)
                                    {
                                        foreach (var pattern in hatch.PatternDefinitions)
                                        {
                                            if (pattern.LineBasePoint != null)
                                            {
                                                pattern.LineBasePoint = new Cad2DPoint(
                                                    pattern.LineBasePoint.X + insertionPt.X,
                                                    pattern.LineBasePoint.Y + insertionPt.Y);
                                            }
                                        }
                                    }

                                    hatch.HatchAngle += insertObj.RotationAngle;
                                }
                            }
                        }
                    }
                }

                cadImage.Save(outputPath);
            }

            Console.WriteLine("Hatch origins adjusted and file saved.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}