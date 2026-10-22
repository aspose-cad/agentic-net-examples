using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";

        // Ensure a placeholder file exists.
        if (!File.Exists(inputPath))
        {
            File.WriteAllBytes(inputPath, new byte[0]);
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("The loaded file is not a CAD image.");
                    return;
                }

                // Adjust hatch origin points (using reflection to avoid missing type errors).
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity.GetType().Name == "CadHatch")
                    {
                        var originProp = entity.GetType().GetProperty("Origin");
                        if (originProp != null && originProp.CanWrite)
                        {
                            // Cad3DPoint constructor: (double x, double y, double z)
                            var pointType = originProp.PropertyType;
                            var ctor = pointType.GetConstructor(new[] { typeof(double), typeof(double), typeof(double) });
                            if (ctor != null)
                            {
                                var originPoint = ctor.Invoke(new object[] { 0.0, 0.0, 0.0 });
                                originProp.SetValue(entity, originPoint);
                            }
                        }
                    }
                }

                // Extract vertices from Polylines and 3DFaces.
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadPolyline polyline)
                    {
                        Console.WriteLine("Polyline vertices:");
                        foreach (var child in polyline.ChildObjects)
                        {
                            // Use dynamic to access possible LocationPoint property.
                            try
                            {
                                dynamic dynChild = child;
                                var loc = dynChild.LocationPoint as Cad3DPoint;
                                if (loc != null)
                                {
                                    PrintPoint(loc);
                                }
                            }
                            catch
                            {
                                // Ignore objects without LocationPoint.
                            }
                        }
                    }
                    else if (entity is Cad3DFace face)
                    {
                        Console.WriteLine("3DFace vertices:");
                        PrintPoint(face.FirstCorner);
                        PrintPoint(face.SecondCorner);
                        PrintPoint(face.ThirdCorner);
                        if (face.FourthCorner != null)
                        {
                            PrintPoint(face.FourthCorner);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static void PrintPoint(Cad3DPoint point)
    {
        if (point != null)
        {
            Console.WriteLine($"  ({point.X}, {point.Y}, {point.Z})");
        }
    }
}
