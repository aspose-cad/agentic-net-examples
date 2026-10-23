using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CadTitleAdjustment
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputPath = "output.pdf";
            try
            {
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            CadImage cadImage;
            try
            {
                cadImage = (CadImage)Image.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CAD file: {ex.Message}");
                return;
            }

            // Move title block
            var titleBlock = cadImage.Entities
                .OfType<CadInsertObject>()
                .FirstOrDefault(i => i.Name == "TitleBlock");

            if (titleBlock != null)
            {
                var original = titleBlock.InsertionPoint;
                titleBlock.InsertionPoint = new Cad3DPoint(original.X + 10, original.Y + 5, original.Z);
                Console.WriteLine($"Title block moved from ({original.X},{original.Y},{original.Z}) to ({titleBlock.InsertionPoint.X},{titleBlock.InsertionPoint.Y},{titleBlock.InsertionPoint.Z})");
            }

            // Helper methods to print geometry
            void PrintLine(CadLine line, string context)
            {
                var p1 = line.FirstPoint;
                var p2 = line.SecondPoint;
                Console.WriteLine($"{context} - Line: ({p1.X},{p1.Y},{p1.Z}) to ({p2.X},{p2.Y},{p2.Z})");
            }

            void PrintLwPolyline(CadLwPolyline pl, string context)
            {
                var pts = pl.Coordinates;
                Console.WriteLine($"{context} - LwPolyline vertices:");
                for (int i = 0; i < pts.Count; i++)
                {
                    var pt = pts[i];
                    Console.WriteLine($"  [{i}] ({pt.X},{pt.Y})");
                }
            }

            void Print3DPolyline(CadPolyline pl, string context)
            {
                var vertices = pl.ChildObjects.OfType<CadVertexBase>().ToList();
                Console.WriteLine($"{context} - 3D Polyline vertices:");
                for (int i = 0; i < vertices.Count; i++)
                {
                    var pt = vertices[i].LocationPoint;
                    Console.WriteLine($"  [{i}] ({pt.X},{pt.Y},{pt.Z})");
                }
            }

            void Print3DFace(Cad3DFace face, string context)
            {
                Console.WriteLine($"{context} - 3DFace corners:");
                Console.WriteLine($"  1: ({face.FirstCorner.X},{face.FirstCorner.Y},{face.FirstCorner.Z})");
                Console.WriteLine($"  2: ({face.SecondCorner.X},{face.SecondCorner.Y},{face.SecondCorner.Z})");
                Console.WriteLine($"  3: ({face.ThirdCorner.X},{face.ThirdCorner.Y},{face.ThirdCorner.Z})");
                Console.WriteLine($"  4: ({face.FourthCorner.X},{face.FourthCorner.Y},{face.FourthCorner.Z})");
            }

            void ProcessEntities(IEnumerable<CadEntityBase> entities, string location)
            {
                foreach (var ent in entities)
                {
                    switch (ent)
                    {
                        case CadLine line:
                            PrintLine(line, location);
                            break;
                        case CadLwPolyline lw:
                            PrintLwPolyline(lw, location);
                            break;
                        case CadPolyline pl3d:
                            Print3DPolyline(pl3d, location);
                            break;
                        case Cad3DFace face:
                            Print3DFace(face, location);
                            break;
                    }
                }
            }

            // Process top-level entities
            ProcessEntities(cadImage.Entities, "ModelSpace");

            // Process block entities
            foreach (var block in cadImage.BlockEntities.ValuesTyped)
            {
                ProcessEntities(block.Entities, $"Block '{block.Name}'");
            }

            // Save as PDF
            try
            {
                var pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Output saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving PDF file: {ex.Message}");
            }
        }
    }
}