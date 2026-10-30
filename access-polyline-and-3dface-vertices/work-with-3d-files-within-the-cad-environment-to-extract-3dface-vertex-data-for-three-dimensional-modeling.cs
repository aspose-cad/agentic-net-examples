using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;

namespace Cad3DFaceExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the source 3D CAD file (DWG, DXF, etc.)
            string sourceFile = "input.dwg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(sourceFile))
            {
                Console.WriteLine($"Error: File \"{sourceFile}\" does not exist.");
                return;
            }

            try
            {
                // Load the CAD file using Aspose.CAD
                using (CadImage cadImage = (CadImage)Image.Load(sourceFile))
                {
                    // Iterate through all 3D Face entities in the CAD drawing
                    var faces = cadImage.Entities.OfType<Cad3DFace>();
                    foreach (Cad3DFace face in faces)
                    {
                        // Extract vertices (each vertex has X, Y, Z coordinates)
                        var v1 = face.FirstCorner;
                        var v2 = face.SecondCorner;
                        var v3 = face.ThirdCorner;
                        var v4 = face.FourthCorner; // May be null for triangular faces

                        // Output vertex coordinates
                        Console.WriteLine("3DFace:");
                        Console.WriteLine($"  Vertex 1: ({v1.X}, {v1.Y}, {v1.Z})");
                        Console.WriteLine($"  Vertex 2: ({v2.X}, {v2.Y}, {v2.Z})");
                        Console.WriteLine($"  Vertex 3: ({v3.X}, {v3.Y}, {v3.Z})");

                        if (v4 != null)
                        {
                            Console.WriteLine($"  Vertex 4: ({v4.X}, {v4.Y}, {v4.Z})");
                        }
                        else
                        {
                            Console.WriteLine("  Vertex 4: (none) – triangular face");
                        }

                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the CAD file: {ex.Message}");
            }
        }
    }
}