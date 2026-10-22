using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

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
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                // Determine a SoftOwner handle (use first layout's block table record handle if available)
                string softOwner = cadImage.Layouts.Values.FirstOrDefault()?.BlockTableRecordHandle ?? string.Empty;

                var newEntities = new List<CadEntityBase>();

                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadText textEntity)
                    {
                        var newText = new CadText
                        {
                            DefaultValue = "Added Text",
                            FirstAlignment = new Cad3DPoint(
                                textEntity.FirstAlignment.X + 10,
                                textEntity.FirstAlignment.Y,
                                textEntity.FirstAlignment.Z),
                            TextHeight = textEntity.TextHeight,
                            SoftOwner = softOwner,
                            LayerName = textEntity.LayerName
                        };
                        newEntities.Add(newText);
                    }
                    else if (entity is CadMText mtextEntity)
                    {
                        var newMText = new CadMText
                        {
                            Text = "Added MText",
                            InsertionPoint = new Cad3DPoint(
                                mtextEntity.InsertionPoint.X + 10,
                                mtextEntity.InsertionPoint.Y,
                                mtextEntity.InsertionPoint.Z),
                            InitialTextHeight = mtextEntity.InitialTextHeight,
                            SoftOwner = softOwner,
                            LayerName = mtextEntity.LayerName
                        };
                        newEntities.Add(newMText);
                    }
                }

                // Append new entities to the drawing
                cadImage.Entities = cadImage.Entities.Concat(newEntities).ToArray();

                var saveOptions = new DwgOptions();

                // Save with options, handling any export errors
                try
                {
                    image.Save(outputPath);
                    Console.WriteLine($"Modified drawing saved to {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Export error: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}