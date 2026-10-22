using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string inputPath;
        if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
        {
            inputPath = args[0];
        }
        else
        {
            inputPath = "circles.txt";
            Console.WriteLine($"No input file argument provided. Using default file: {inputPath}");
        }

        if (!File.Exists(inputPath))
        {
            try
            {
                File.WriteAllLines(inputPath, new[] { "1.0", "2.5", "3.75" });
                Console.WriteLine($"Placeholder input file created at {inputPath} with sample radii.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                return;
            }
        }

        bool anyCircle = false;
        try
        {
            foreach (var line in File.ReadLines(inputPath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (!double.TryParse(line.Trim(), out double radius))
                {
                    Console.Error.WriteLine($"Invalid radius value: '{line}'. Skipping.");
                    continue;
                }

                double area = Math.PI * radius * radius;
                double circumference = 2.0 * Math.PI * radius;

                Console.WriteLine($"Circle with radius {radius}:");
                Console.WriteLine($"  Area: {area}");
                Console.WriteLine($"  Circumference: {circumference}");
                anyCircle = true;
            }

            if (!anyCircle)
            {
                Console.WriteLine("No valid circle radii were found in the input file.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing file: {ex.Message}");
        }
    }
}
