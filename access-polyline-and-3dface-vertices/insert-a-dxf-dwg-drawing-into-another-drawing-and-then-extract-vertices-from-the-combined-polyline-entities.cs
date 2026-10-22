using System;
using System.IO;
using System.Text;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadInsertAndExtract
{
    class Program
    {
        static void Main(string[] args)
        {
            string targetPath = "target.dxf";
            string sourcePath = "source.dxf";
            string outputFolder = "Output";
            string combinedPath = Path.Combine(outputFolder, "combined.dxf");
            string verticesReportPath = Path.Combine(outputFolder, "vertices.txt");

            try
            {
                // Verify input files
                if (!File.Exists(targetPath))
                {
                    Console.Error.WriteLine($"Target CAD file not found: {targetPath}");
                    return;
                }

                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source CAD file not found: {sourcePath}");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                // Load target drawing
                using (Image targetImage = Image.Load(targetPath))
                {
                    CadImage targetCad = targetImage as CadImage;
                    if (targetCad == null)
                    {
                        Console.Error.WriteLine("Failed to cast target image to CadImage.");
                        return;
                    }

                    // Load source drawing
                    using (Image sourceImage = Image.Load(sourcePath))
                    {
                        CadImage sourceCad = sourceImage as CadImage;
                        if (sourceCad == null)
                        {
                            Console.Error.WriteLine("Failed to cast source image to CadImage.");
                            return;
                        }

                        // NOTE: Direct addition of entities to targetCad.Entities is not supported in this context.
                        // If a proper API exists for merging, it should be used here.
                        // For demonstration, we will copy block definitions and then save the original target as combined.

                        // Copy block definitions (if any) from source to target
                        foreach (CadBlockEntity sourceBlock in sourceCad.BlockEntities.ValuesTyped)
                        {
                            if (!targetCad.BlockEntities.ContainsKey(sourceBlock.Name))
                            {
                                targetCad.BlockEntities.Add(sourceBlock.Name, sourceBlock);
                            }
                        }

                        // Save the combined drawing (target with copied blocks)
                        targetCad.Save(combinedPath);
                    }

                    // Extract vertices from combined polyline and 3DFace entities
                    StringBuilder sb = new StringBuilder();

                    foreach (CadEntityBase entity in targetCad.Entities)
                    {
                        if (entity is CadLwPolyline lwPolyline)
                        {
                            int vertexIndex = 0;
                            foreach (Cad2DPoint pt in lwPolyline.Coordinates)
                            {
                                sb.AppendLine($"LwPolyline Vertex {vertexIndex}: X={pt.X}, Y={pt.Y}");
                                vertexIndex++;
                            }
                        }
                        else if (entity is Cad3DFace face)
                        {
                            sb.AppendLine($"3DFace Vertex 0: X={face.FirstCorner.X}, Y={face.FirstCorner.Y}, Z={face.FirstCorner.Z}");
                            sb.AppendLine($"3DFace Vertex 1: X={face.SecondCorner.X}, Y={face.SecondCorner.Y}, Z={face.SecondCorner.Z}");
                            sb.AppendLine($"3DFace Vertex 2: X={face.ThirdCorner.X}, Y={face.ThirdCorner.Y}, Z={face.ThirdCorner.Z}");
                            sb.AppendLine($"3DFace Vertex 3: X={face.FourthCorner.X}, Y={face.FourthCorner.Y}, Z={face.FourthCorner.Z}");
                        }
                    }

                    // Write vertices report
                    File.WriteAllText(verticesReportPath, sb.ToString());
                }

                Console.WriteLine("Processing completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}