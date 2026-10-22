using System;

namespace CircleCircumferenceCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Enter the radius of the circle: ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input) || !double.TryParse(input, out double radius) || radius < 0)
                {
                    Console.Error.WriteLine("Invalid radius value.");
                    return;
                }

                // Compute circumference: C = 2 * π * r
                double circumference = 2.0 * Math.PI * radius;

                Console.WriteLine($"Circle radius: {radius}");
                Console.WriteLine($"Circle circumference: {circumference}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
