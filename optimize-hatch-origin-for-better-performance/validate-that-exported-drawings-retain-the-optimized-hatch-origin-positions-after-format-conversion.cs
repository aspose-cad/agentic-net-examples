using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CadHatchOriginValidation
{
    struct PointF
    {
        public float X;
        public float Y;
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: CadHatchOriginValidation <input-cad-file> <output-dxf-file>");
                return;
            }

            string inputFilePath = args[0];
            string outputFilePath = args[1];

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Input file does not exist: {inputFilePath}");
                return;
            }

            string outputDirectory = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            try
            {
                using (CadImage originalImage = (CadImage)Image.Load(inputFilePath))
                {
                    List<PointF> originalOrigins = ExtractHatchOrigins(originalImage);

                    DxfOptions dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };

                    originalImage.Save(outputFilePath, dxfOptions);

                    using (CadImage convertedImage = (CadImage)Image.Load(outputFilePath))
                    {
                        List<PointF> convertedOrigins = ExtractHatchOrigins(convertedImage);

                        bool match = CompareOrigins(originalOrigins, convertedOrigins);
                        Console.WriteLine(match
                            ? "Hatch origins are preserved after conversion."
                            : "Hatch origins differ after conversion.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during processing: {ex.Message}");
            }
        }

        private static List<PointF> ExtractHatchOrigins(CadImage cadImage)
        {
            List<PointF> origins = new List<PointF>();

            foreach (var entity in cadImage.Entities)
            {
                // Use string comparison for entity type name to avoid missing enum member
                if (entity.TypeName != null && entity.TypeName.ToString().Equals("HATCH", StringComparison.OrdinalIgnoreCase))
                {
                    PropertyInfo originProp = entity.GetType().GetProperty("Origin", BindingFlags.Public | BindingFlags.Instance);
                    if (originProp != null)
                    {
                        object originValue = originProp.GetValue(entity);
                        if (originValue != null)
                        {
                            PropertyInfo xProp = originValue.GetType().GetProperty("X");
                            PropertyInfo yProp = originValue.GetType().GetProperty("Y");
                            if (xProp != null && yProp != null)
                            {
                                float x = Convert.ToSingle(xProp.GetValue(originValue));
                                float y = Convert.ToSingle(yProp.GetValue(originValue));
                                origins.Add(new PointF { X = x, Y = y });
                            }
                        }
                    }
                }
            }

            return origins;
        }

        private static bool CompareOrigins(List<PointF> listA, List<PointF> listB)
        {
            const float tolerance = 0.001f;

            if (listA.Count != listB.Count)
                return false;

            for (int i = 0; i < listA.Count; i++)
            {
                float deltaX = Math.Abs(listA[i].X - listB[i].X);
                float deltaY = Math.Abs(listA[i].Y - listB[i].Y);
                if (deltaX > tolerance || deltaY > tolerance)
                    return false;
            }

            return true;
        }
    }
}