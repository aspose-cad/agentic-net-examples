using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "circles.txt";

            if (!File.Exists(inputPath))
            {
                // Create a minimal placeholder input file with sample radii
                File.WriteAllLines(inputPath, new[] { "1.0", "2.5", "3.0" });
                Console.WriteLine($"Created placeholder input file: {inputPath}");
            }

            List<double> circleAreas = new List<double>();

            // Read radii from the file (one radius per line)
            foreach (var line in File.ReadAllLines(inputPath))
            {
                if (double.TryParse(line.Trim(), out double radius))
                {
                    double area = Math.PI * radius * radius;
                    circleAreas.Add(area);
                }
            }

            // Output results
            for (int i = 0; i < circleAreas.Count; i++)
            {
                Console.WriteLine($"Circle {i + 1} area: {circleAreas[i]}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
