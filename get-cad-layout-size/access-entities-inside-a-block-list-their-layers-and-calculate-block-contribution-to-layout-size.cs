using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string filePath = "sample.dwg";

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(filePath))
            {
                CadImage cadImage = (CadImage)image;

                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}");

                    double minX = double.MaxValue;
                    double minY = double.MaxValue;
                    double maxX = double.MinValue;
                    double maxY = double.MinValue;

                    foreach (CadEntityBase entity in block.Entities)
                    {
                        Console.WriteLine($"  Entity Type: {entity.GetType().Name}, Layer: {entity.LayerName}");

                        if (entity is CadLine line)
                        {
                            UpdateBounds(line.FirstPoint.X, line.FirstPoint.Y, ref minX, ref minY, ref maxX, ref maxY);
                            UpdateBounds(line.SecondPoint.X, line.SecondPoint.Y, ref minX, ref minY, ref maxX, ref maxY);
                        }
                        else if (entity is CadLwPolyline polyline)
                        {
                            foreach (var pt in polyline.Coordinates)
                            {
                                UpdateBounds(pt.X, pt.Y, ref minX, ref minY, ref maxX, ref maxY);
                            }
                        }
                        else if (entity is Cad3DFace face)
                        {
                            UpdateBounds(face.FirstCorner.X, face.FirstCorner.Y, ref minX, ref minY, ref maxX, ref maxY);
                            UpdateBounds(face.SecondCorner.X, face.SecondCorner.Y, ref minX, ref minY, ref maxX, ref maxY);
                            UpdateBounds(face.ThirdCorner.X, face.ThirdCorner.Y, ref minX, ref minY, ref maxX, ref maxY);
                            UpdateBounds(face.FourthCorner.X, face.FourthCorner.Y, ref minX, ref minY, ref maxX, ref maxY);
                        }
                    }

                    double width = maxX - minX;
                    double height = maxY - minY;

                    Console.WriteLine($"  Bounding Box: Min({minX}, {minY}) Max({maxX}, {maxY})");
                    Console.WriteLine($"  Approximate Size: Width = {width}, Height = {height}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while processing the CAD file: {ex.Message}");
        }
    }

    static void UpdateBounds(double x, double y, ref double minX, ref double minY, ref double maxX, ref double maxY)
    {
        if (x < minX) minX = x;
        if (y < minY) minY = y;
        if (x > maxX) maxX = x;
        if (y > maxY) maxY = y;
    }
}