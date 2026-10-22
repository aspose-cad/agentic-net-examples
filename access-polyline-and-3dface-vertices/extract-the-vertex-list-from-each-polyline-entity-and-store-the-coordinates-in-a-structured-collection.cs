using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Path to the source CAD file
        string inputPath = "input.dwg";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD file using Aspose.CAD
            using (Image image = Image.Load(inputPath))
            {
                // Cast the loaded image to CadImage to access CAD-specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Collection to store vertices of each polyline entity
                List<List<Tuple<double, double, double>>> polylinesVertices = new List<List<Tuple<double, double, double>>>();

                // Iterate over all top‑level CAD entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Handle lightweight polylines (LWPOLYLINE)
                    if (entity is CadLwPolyline lwPolyline)
                    {
                        List<Tuple<double, double, double>> vertices = new List<Tuple<double, double, double>>();
                        foreach (Cad2DPoint vertex in lwPolyline.Coordinates)
                        {
                            double x = vertex.X;
                            double y = vertex.Y;
                            double z = 0.0;
                            vertices.Add(Tuple.Create(x, y, z));
                        }
                        polylinesVertices.Add(vertices);
                    }
                    // Handle 3D polylines (if present) using reflection to avoid missing compile‑time type
                    else if (entity.GetType().Name == "CadPolyline")
                    {
                        PropertyInfo verticesProperty = entity.GetType().GetProperty("Vertices");
                        if (verticesProperty != null)
                        {
                            object verticesObj = verticesProperty.GetValue(entity);
                            if (verticesObj is IEnumerable<Cad2DPoint> vertexCollection)
                            {
                                List<Tuple<double, double, double>> vertices = new List<Tuple<double, double, double>>();
                                foreach (Cad2DPoint vertex in vertexCollection)
                                {
                                    vertices.Add(Tuple.Create(vertex.X, vertex.Y, 0.0));
                                }
                                polylinesVertices.Add(vertices);
                            }
                        }
                    }
                }

                // Output the extracted vertex lists
                for (int i = 0; i < polylinesVertices.Count; i++)
                {
                    Console.WriteLine($"Polyline {i + 1}:");
                    List<Tuple<double, double, double>> vertices = polylinesVertices[i];
                    for (int j = 0; j < vertices.Count; j++)
                    {
                        var v = vertices[j];
                        Console.WriteLine($"  Vertex {j + 1}: X={v.Item1}, Y={v.Item2}, Z={v.Item3}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}