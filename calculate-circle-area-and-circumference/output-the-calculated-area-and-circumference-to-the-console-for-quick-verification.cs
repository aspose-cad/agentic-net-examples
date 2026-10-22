using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.txt";
            if (!File.Exists(inputPath))
            {
                // Create a minimal input file with sample radius values
                File.WriteAllLines(inputPath, new[] { "1.0", "2.5", "3.75" });
                Console.WriteLine($"Created placeholder input file: {inputPath}");
            }

            string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            string[] lines = File.ReadAllLines(inputPath);
            foreach (string line in lines)
            {
                if (double.TryParse(line.Trim(), out double radius))
                {
                    double area = Math.PI * radius * radius;
                    double circumference = 2 * Math.PI * radius;

                    Console.WriteLine($"Circle found with radius: {radius}");
                    Console.WriteLine($"Area: {area}");
                    Console.WriteLine($"Circumference: {circumference}");
                }
                else
                {
                    Console.Error.WriteLine($"Invalid radius value: '{line}'");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
