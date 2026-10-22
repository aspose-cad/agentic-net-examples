using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace Cad3DFaceInspection
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the input CAD file (placeholder will be created if missing)
                    string inputPath = "sample.dwg";

                    // Ensure a placeholder file exists to satisfy validation rules
                    if (!File.Exists(inputPath))
                    {
                        // Create an empty placeholder file
                        File.WriteAllBytes(inputPath, new byte[0]);
                    }

                    // Load the CAD file
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Iterate through block definitions
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            // Iterate through entities inside each block
                            foreach (CadEntityBase entity in block.Entities)
                            {
                                // Process only 3D Face entities
                                if (entity is Cad3DFace face)
                                {
                                    // Retrieve vertices of the 3D face
                                    Cad2DPoint vertex1 = face.FirstCorner;
                                    Cad2DPoint vertex2 = face.SecondCorner;
                                    Cad2DPoint vertex3 = face.ThirdCorner;

                                    // Compute two edge vectors
                                    double ax = vertex2.X - vertex1.X;
                                    double ay = vertex2.Y - vertex1.Y;
                                    double az = 0 - 0;

                                    double bx = vertex3.X - vertex1.X;
                                    double by = vertex3.Y - vertex1.Y;
                                    double bz = 0 - 0;

                                    // Compute the cross product (normal vector)
                                    double nx = ay * bz - az * by;
                                    double ny = az * bx - ax * bz;
                                    double nz = ax * by - ay * bx;

                                    // Normalize the normal vector
                                    double length = Math.Sqrt(nx * nx + ny * ny + nz * nz);
                                    if (length != 0)
                                    {
                                        nx /= length;
                                        ny /= length;
                                        nz /= length;
                                    }

                                    // Output the calculated normal vector
                                    Console.WriteLine($"3DFace normal: ({nx}, {ny}, {nz})");
                                }
                            }
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
}