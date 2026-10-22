using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadTitleManipulation
{
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
                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Move all existing entities down by 100 units
                    double offsetY = -100.0;
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        switch (entity)
                        {
                            case CadLine line:
                                {
                                    Cad3DPoint first = line.FirstPoint;
                                    Cad3DPoint second = line.SecondPoint;
                                    line.FirstPoint = new Cad3DPoint(first.X, first.Y + offsetY, first.Z);
                                    line.SecondPoint = new Cad3DPoint(second.X, second.Y + offsetY, second.Z);
                                    break;
                                }
                            case CadLwPolyline polyline:
                                {
                                    List<Cad2DPoint> points = polyline.Coordinates;
                                    for (int i = 0; i < points.Count; i++)
                                    {
                                        Cad2DPoint pt = points[i];
                                        points[i] = new Cad2DPoint(pt.X, pt.Y + offsetY);
                                    }
                                    // No need to reassign; the list is modified in place
                                    break;
                                }
                            case CadMText mtext:
                                {
                                    Cad3DPoint ip = mtext.InsertionPoint;
                                    mtext.InsertionPoint = new Cad3DPoint(ip.X, ip.Y + offsetY, ip.Z);
                                    break;
                                }
                            // Additional entity types can be handled similarly if needed
                        }
                    }

                    // Add a title entity (MText) at the origin
                    CadMText titleEntity = new CadMText
                    {
                        Text = "Drawing Title",
                        InsertionPoint = new Cad3DPoint(0, 0, 0)
                    };
                    cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { titleEntity }).ToArray();

                    // Locate the index of the title entity
                    int titleIndex = -1;
                    CadEntityBase[] entitiesArray = cadImage.Entities.ToArray();
                    for (int i = 0; i < entitiesArray.Length; i++)
                    {
                        if (entitiesArray[i] is CadMText txt && txt.Text == "Drawing Title")
                        {
                            titleIndex = i;
                            break;
                        }
                    }

                    // NOTE: CadImage.RemoveEntityAt is not available in the current Aspose.CAD API.
                    // To delete the title entity, we recreate the entity collection without it.
                    if (titleIndex >= 0)
                    {
                        CadEntityBase[] filtered = entitiesArray
                            .Where((e, idx) => idx != titleIndex)
                            .ToArray();
                        cadImage.Entities = filtered;
                    }

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
}