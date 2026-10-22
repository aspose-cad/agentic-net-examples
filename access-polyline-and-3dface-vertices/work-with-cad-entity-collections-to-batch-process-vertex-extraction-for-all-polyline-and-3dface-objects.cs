using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                List<Cad2DPoint> allVertices = new List<Cad2DPoint>();

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLwPolyline polyline)
                    {
                        foreach (Cad2DPoint vertex in polyline.Coordinates)
                        {
                            allVertices.Add(vertex);
                        }
                    }
                    else if (entity is Cad3DFace face)
                    {
                        allVertices.Add(face.FirstCorner);
                        allVertices.Add(face.SecondCorner);
                        allVertices.Add(face.ThirdCorner);
                        if (face.FourthCorner != null)
                        {
                            allVertices.Add(face.FourthCorner);
                        }
                    }
                }

                Console.WriteLine($"Extracted total vertices: {allVertices.Count}");

                int index = 0;
                foreach (Cad2DPoint v in allVertices)
                {
                    Console.WriteLine($"Vertex {index++}: X={v.X}, Y={v.Y}, Z={0}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}