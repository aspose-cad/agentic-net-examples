using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        string cadFilePath = "sample.txt";

        // Ensure the input file exists; create a minimal placeholder if missing
        if (!File.Exists(cadFilePath))
        {
            File.WriteAllText(cadFilePath, "// Placeholder CAD data. Format: CIRCLE x y radius\n");
        }

        try
        {
            var circles = new List<Circle>();

            foreach (var line in File.ReadLines(cadFilePath))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("//"))
                    continue;

                var parts = trimmed.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 4 && parts[0].Equals("CIRCLE", StringComparison.OrdinalIgnoreCase))
                {
                    if (double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double x) &&
                        double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double y) &&
                        double.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out double r))
                    {
                        circles.Add(new Circle { CenterX = x, CenterY = y, Radius = r });
                    }
                }
            }

            if (circles.Count == 0)
            {
                Console.WriteLine("No circle entities were found in the drawing.");
            }
            else
            {
                foreach (var c in circles)
                {
                    Console.WriteLine($"Circle found: Center=({c.CenterX}, {c.CenterY}), Radius={c.Radius}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Processing failed: {ex.Message}");
        }
    }

    class Circle
    {
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double Radius { get; set; }
    }
}