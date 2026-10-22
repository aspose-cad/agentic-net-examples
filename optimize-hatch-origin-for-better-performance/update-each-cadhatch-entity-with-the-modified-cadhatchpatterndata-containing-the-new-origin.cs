using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadHatchUpdateSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input and output file paths (replace with actual paths)
            string inputFilePath = "input.dwg";
            string outputFilePath = "output.pdf";

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            try
            {
                using (CadImage cadImage = (CadImage)Image.Load(inputFilePath))
                {
                    foreach (var entity in cadImage.Entities)
                    {
                        // Identify hatch entities by type name (avoid direct type reference)
                        string typeName = entity.GetType().Name;
                        if (!typeName.Contains("Hatch", StringComparison.OrdinalIgnoreCase))
                            continue;

                        // Get the PatternData property
                        PropertyInfo patternProp = entity.GetType().GetProperty("PatternData", BindingFlags.Public | BindingFlags.Instance);
                        if (patternProp == null) continue;

                        object patternData = patternProp.GetValue(entity);
                        if (patternData == null) continue;

                        // Get the Origin property of the pattern data
                        PropertyInfo originProp = patternData.GetType().GetProperty("Origin", BindingFlags.Public | BindingFlags.Instance);
                        if (originProp == null) continue;

                        Type pointType = originProp.PropertyType;
                        object pointInstance = null;

                        // Try to find a suitable constructor (double, double) or (double, double, double)
                        ConstructorInfo[] ctors = pointType.GetConstructors();
                        foreach (var ctor in ctors)
                        {
                            ParameterInfo[] parms = ctor.GetParameters();
                            if (parms.Length == 2 && parms.All(p => p.ParameterType == typeof(double)))
                            {
                                pointInstance = ctor.Invoke(new object[] { 0.0, 0.0 });
                                break;
                            }
                            if (parms.Length == 3 && parms.All(p => p.ParameterType == typeof(double)))
                            {
                                pointInstance = ctor.Invoke(new object[] { 0.0, 0.0, 0.0 });
                                break;
                            }
                        }

                        // Fallback: create default instance if possible
                        if (pointInstance == null && pointType.IsValueType)
                            pointInstance = Activator.CreateInstance(pointType);

                        if (pointInstance != null)
                            originProp.SetValue(patternData, pointInstance);
                    }

                    // Save the modified CAD as PDF
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputFilePath, pdfOptions);
                }

                Console.WriteLine($"Successfully saved updated CAD to PDF: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}