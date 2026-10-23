using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Obj;
using Aspose.CAD.FileFormats.Obj.Elements;
using Aspose.CAD.FileFormats.Obj.VertexData;
using Aspose.CAD.FileFormats.Obj.VertexData.Index;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.obj";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"File not found: {inputPath}");
                return;
            }

            // Load OBJ as ObjImage
            var objImage = Image.Load(inputPath) as ObjImage;
            if (objImage == null)
            {
                Console.Error.WriteLine("Failed to load OBJ file as ObjImage.");
                return;
            }

            var root = objImage.ObjRoot;
            if (root == null)
            {
                Console.Error.WriteLine("OBJ root is null.");
                return;
            }

            // Extract vertices
            List<ObjVertex> vertices = root.Vertices;
            Console.WriteLine($"Vertex count: {vertices?.Count ?? 0}");
            for (int i = 0; i < Math.Min(5, vertices?.Count ?? 0); i++)
            {
                var v = vertices[i];
                Console.WriteLine($"Vertex {i}: ({v.X}, {v.Y}, {v.Z})");
            }

            // Extract faces
            List<ObjShape> shapes = root.Shapes;
            Console.WriteLine($"Shape count: {shapes?.Count ?? 0}");
            int faceIndex = 0;
            foreach (var shape in shapes ?? new List<ObjShape>())
            {
                var faces = shape.Faces;
                foreach (var face in faces ?? new List<ObjFace>())
                {
                    Console.WriteLine($"Face {faceIndex} in shape '{shape.Name}':");
                    foreach (var idx in face.VertexTextureNormals ?? new List<ObjVertexTextureNormalIndex>())
                    {
                        Console.Write($"{idx.VertexIndex} ");
                    }
                    Console.WriteLine();
                    faceIndex++;
                    if (faceIndex >= 5) break;
                }
                if (faceIndex >= 5) break;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}