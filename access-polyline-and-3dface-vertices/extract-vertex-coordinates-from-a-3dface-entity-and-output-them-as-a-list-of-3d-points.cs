using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace Cad3DFaceVertexExtraction
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file (DWG, DXF, etc.)
                string sourcePath = "input.dwg";

                // Verify that the file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"File not found: {sourcePath}");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast the loaded image to CadImage to access CAD‑specific members
                    CadImage cadImage = (CadImage)image;

                    // List to hold extracted 3D points
                    List<(double X, double Y, double Z)> vertices = new List<(double, double, double)>();

                    // Process top‑level entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity is Cad3DFace face)
                        {
                            vertices.Add((face.FirstCorner.X, face.FirstCorner.Y, face.FirstCorner.Z));
                            vertices.Add((face.SecondCorner.X, face.SecondCorner.Y, face.SecondCorner.Z));
                            vertices.Add((face.ThirdCorner.X, face.ThirdCorner.Y, face.ThirdCorner.Z));
                            if (face.FourthCorner != null)
                            {
                                vertices.Add((face.FourthCorner.X, face.FourthCorner.Y, face.FourthCorner.Z));
                            }
                        }
                    }

                    // Process entities inside block definitions
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            if (entity is Cad3DFace face)
                            {
                                vertices.Add((face.FirstCorner.X, face.FirstCorner.Y, face.FirstCorner.Z));
                                vertices.Add((face.SecondCorner.X, face.SecondCorner.Y, face.SecondCorner.Z));
                                vertices.Add((face.ThirdCorner.X, face.ThirdCorner.Y, face.ThirdCorner.Z));
                                if (face.FourthCorner != null)
                                {
                                    vertices.Add((face.FourthCorner.X, face.FourthCorner.Y, face.FourthCorner.Z));
                                }
                            }
                        }
                    }

                    // Output the extracted vertices
                    foreach (var point in vertices)
                    {
                        Console.WriteLine($"X: {point.X}, Y: {point.Y}, Z: {point.Z}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}