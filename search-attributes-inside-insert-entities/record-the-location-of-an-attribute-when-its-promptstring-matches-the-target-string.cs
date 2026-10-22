using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            const string inputFilePath = "attributes.txt";

            // Create a minimal placeholder file if it does not exist
            if (!File.Exists(inputFilePath))
            {
                File.WriteAllLines(inputFilePath, new[]
                {
                    "TARGET,10,20,0",
                    "OTHER,5,5,0",
                    "target,30,40,0"
                });
            }

            const string targetPrompt = "TARGET";

            List<Point3D> matchingLocations = new List<Point3D>();

            foreach (var line in File.ReadLines(inputFilePath))
            {
                var parts = line.Split(',');
                if (parts.Length < 4) continue;

                var prompt = parts[0].Trim();
                if (string.Equals(prompt, targetPrompt, StringComparison.OrdinalIgnoreCase))
                {
                    if (double.TryParse(parts[1], out double x) &&
                        double.TryParse(parts[2], out double y) &&
                        double.TryParse(parts[3], out double z))
                    {
                        matchingLocations.Add(new Point3D { X = x, Y = y, Z = z });
                    }
                }
            }

            Console.WriteLine($"Found {matchingLocations.Count} attribute(s) with PromptString \"{targetPrompt}\":");
            foreach (var point in matchingLocations)
            {
                Console.WriteLine($"Location -> X: {point.X}, Y: {point.Y}, Z: {point.Z}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Processing failed: {ex.Message}");
        }
    }

    class Point3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}