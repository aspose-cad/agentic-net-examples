using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = args.Length > 0 ? args[0] : "sample.dwg";
        string outputFolder = "output";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // List layers and their visibility status
                Console.WriteLine("Layers:");
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool isOff = layer.ColorId < 0;
                    bool isFrozen = (layer.Flags & 1) != 0;
                    bool isLocked = (layer.Flags & 0x10) != 0;
                    Console.WriteLine($"  Name: {layer.Name}, ColorId: {layer.ColorId}, Off: {isOff}, Frozen: {isFrozen}, Locked: {isLocked}");
                }

                // Inspect block definitions
                Console.WriteLine("\nBlock Definitions:");
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block Name: {block.Name}, Entity Count: {block.Entities.Count}");
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        PrintEntityInfo(entity, "    ", cadImage);
                    }
                }

                // Inspect top‑level entities (including block references)
                Console.WriteLine("\nTop‑Level Entities:");
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    PrintEntityInfo(entity, "  ", cadImage);
                }

                // Optional: save a PDF copy of the drawing
                string pdfPath = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(inputPath) + ".pdf");
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(pdfPath, pdfOptions);
                Console.WriteLine($"\nSaved PDF to: {pdfPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static void PrintEntityInfo(CadEntityBase entity, string indent, CadImage parentImage)
    {
        if (entity is CadLine line)
        {
            Console.WriteLine($"{indent}Line: Start({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) " +
                              $"End({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z}) " +
                              $"Layer: {line.LayerName}");
        }
        else if (entity is CadInsertObject insert)
        {
            string blockName = insert.Name ?? insert.OriginalBlockName;
            Console.Write($"{indent}Insert: BlockName = {blockName ?? "<null>"}");
            if (!string.IsNullOrEmpty(blockName))
            {
                bool blockExists = parentImage.BlockEntities.ContainsKey(blockName);
                Console.WriteLine(blockExists ? " (found)" : " (missing)");
            }
            else
            {
                Console.WriteLine(" (no name)");
            }
        }
        else if (entity is CadArc arc)
        {
            Console.WriteLine($"{indent}Arc: Center({arc.CenterPoint.X}, {arc.CenterPoint.Y}, {arc.CenterPoint.Z}) " +
                              $"Radius: {arc.Radius} StartAngle: {arc.StartAngle} EndAngle: {arc.EndAngle} " +
                              $"Layer: {arc.LayerName}");
        }
        else if (entity is CadCircle circle)
        {
            Console.WriteLine($"{indent}Circle: Center({circle.CenterPoint.X}, {circle.CenterPoint.Y}, {circle.CenterPoint.Z}) " +
                              $"Radius: {circle.Radius} Layer: {circle.LayerName}");
        }
        else if (entity is CadLwPolyline polyline)
        {
            Console.Write($"{indent}LwPolyline: Points = ");
            foreach (Cad2DPoint pt in polyline.Coordinates)
            {
                Console.Write($"({pt.X}, {pt.Y}) ");
            }
            Console.WriteLine($"Layer: {polyline.LayerName}");
        }
        else if (entity is Cad3DFace face)
        {
            Console.WriteLine($"{indent}3DFace: " +
                              $"P1({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z}) " +
                              $"P2({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z}) " +
                              $"P3({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z}) " +
                              $"P4({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z}) " +
                              $"Layer: {face.LayerName}");
        }
        else
        {
            Console.WriteLine($"{indent}Entity Type: {entity.GetType().Name} Layer: {entity.LayerName}");
        }
    }
}