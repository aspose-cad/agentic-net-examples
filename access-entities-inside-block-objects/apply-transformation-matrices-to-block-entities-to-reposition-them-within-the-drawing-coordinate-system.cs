using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadBlockTransformation
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input and output file paths
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPath = "output.dwg";

                    // Translation offsets to apply to block entities
                    double offsetX = 100.0;
                    double offsetY = 50.0;
                    double offsetZ = 0.0;

                    // Load the CAD drawing
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD object model
                        CadImage cadImage = (CadImage)image;

                        // Iterate over each block definition in the drawing
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                            {

                            // Iterate over entities contained in the block
                            foreach (CadEntityBase entity in block.Entities)
                            {
                                // Apply translation to line entities
                                if (entity is CadLine line)
                                {
                                    Cad3DPoint first = line.FirstPoint;
                                    Cad3DPoint second = line.SecondPoint;

                                    line.FirstPoint = new Cad3DPoint(first.X + offsetX, first.Y + offsetY, first.Z + offsetZ);
                                    line.SecondPoint = new Cad3DPoint(second.X + offsetX, second.Y + offsetY, second.Z + offsetZ);
                                }
                                // Apply translation to lightweight polyline entities
                                else if (entity is CadLwPolyline polyline)
                                {
                                    IList<Cad2DPoint> points = polyline.Coordinates;
                                    for (int i = 0; i < points.Count; i++)
                                    {
                                        Cad2DPoint pt = points[i];
                                        points[i] = new Cad2DPoint(pt.X + offsetX, pt.Y + offsetY);
                                    }
                                }
                                // Apply translation to 3D face entities
                                else if (entity is Cad3DFace face)
                                {
                                    Cad3DPoint c1 = face.FirstCorner;
                                    Cad3DPoint c2 = face.SecondCorner;
                                    Cad3DPoint c3 = face.ThirdCorner;
                                    Cad3DPoint c4 = face.FourthCorner;

                                    face.FirstCorner = new Cad3DPoint(c1.X + offsetX, c1.Y + offsetY, c1.Z + offsetZ);
                                    face.SecondCorner = new Cad3DPoint(c2.X + offsetX, c2.Y + offsetY, c2.Z + offsetZ);
                                    face.ThirdCorner = new Cad3DPoint(c3.X + offsetX, c3.Y + offsetY, c3.Z + offsetZ);
                                    face.FourthCorner = new Cad3DPoint(c4.X + offsetX, c4.Y + offsetY, c4.Z + offsetZ);
                                }
                                // Additional entity types can be handled here as needed
                            }
                        }

                        // Save the modified drawing to a new file
                        cadImage.Save(outputPath);
                    }
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}