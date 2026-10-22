using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: <program> <input CAD file> <output PDF file>");
                return;
            }

            string inputPath = args[0];
            string outputPath = args[1];

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                Console.WriteLine("Top‑level entities:");
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    switch (entity)
                    {
                        case CadLine line:
                            Console.WriteLine($"Line: ({line.FirstPoint.X}, {line.FirstPoint.Y}) -> ({line.SecondPoint.X}, {line.SecondPoint.Y})");
                            break;
                        case CadCircle circle when circle is not CadArc:
                            Console.WriteLine($"Circle: Center=({circle.CenterPoint.X}, {circle.CenterPoint.Y}), Radius={circle.Radius}");
                            break;
                        case CadArc arc:
                            Console.WriteLine($"Arc: Center=({arc.CenterPoint.X}, {arc.CenterPoint.Y}), Radius={arc.Radius}, StartAngle={arc.StartAngle}, EndAngle={arc.EndAngle}");
                            break;
                        case CadLwPolyline polyline:
                            Console.WriteLine($"LwPolyline: VertexCount={polyline.Coordinates.Count}");
                            break;
                        case Cad3DFace face:
                            Console.WriteLine("3DFace: Four corners defined.");
                            break;
                        case CadInsertObject insert:
                            Console.WriteLine($"Insert: BlockName={insert.Name}, InsertionPoint=({insert.InsertionPoint.X}, {insert.InsertionPoint.Y})");
                            break;
                        default:
                            Console.WriteLine($"Other entity type: {entity.GetType().Name}");
                            break;
                    }
                }

                Console.WriteLine("\nBlock definitions:");
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}, EntityCount={block.Entities.Count}");
                }

                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"\nSaved PDF to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}