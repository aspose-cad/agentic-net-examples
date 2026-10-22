using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CircleMetricsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = "Input/sample.dxf";
            string outputDirectory = "Output";
            string reportFilePath = Path.Combine(outputDirectory, "CircleMetricsReport.txt");

            try
            {
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Error: The input file \"{inputFilePath}\" does not exist.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(outputDirectory))
                {
                    Console.Error.WriteLine("Error: Output directory path is empty or whitespace.");
                    return;
                }

                Directory.CreateDirectory(outputDirectory);

                using (Image loadedImage = Image.Load(inputFilePath))
                {
                    CadImage cadImage = loadedImage as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Error: The loaded file could not be interpreted as a CAD image.");
                        return;
                    }

                    List<string> calculationResults = new List<string>();

                    foreach (var entity in cadImage.Entities)
                    {
                        if (entity is CadCircle circleEntity)
                        {
                            double radius = circleEntity.Radius;
                            double area = Math.PI * radius * radius;
                            double circumference = 2.0 * Math.PI * radius;

                            string resultLine = string.Format(
                                "Circle Center: ({0:F3}, {1:F3}), Radius: {2:F3} => Area: {3:F3}, Circumference: {4:F3}",
                                circleEntity.CenterPoint.X,
                                circleEntity.CenterPoint.Y,
                                radius,
                                area,
                                circumference);

                            calculationResults.Add(resultLine);
                        }
                    }

                    if (calculationResults.Count == 0)
                    {
                        Console.WriteLine("No circle entities were found in the provided CAD file.");
                    }
                    else
                    {
                        Console.WriteLine("Circle metrics calculated:");
                        foreach (string line in calculationResults)
                        {
                            Console.WriteLine(line);
                        }

                        using (StreamWriter writer = new StreamWriter(reportFilePath, false))
                        {
                            writer.WriteLine("Circle Metrics Report");
                            writer.WriteLine("=====================");
                            writer.WriteLine($"Source CAD file: {Path.GetFullPath(inputFilePath)}");
                            writer.WriteLine($"Report generated on: {DateTime.Now}");
                            writer.WriteLine();
                            foreach (string line in calculationResults)
                            {
                                writer.WriteLine(line);
                            }
                        }

                        Console.WriteLine($"A detailed report has been saved to \"{reportFilePath}\".");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
