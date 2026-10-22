using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter the radius of the circle: ");
        string input = Console.ReadLine();

        try
        {
            double radius = double.Parse(input);
            if (radius < 0)
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius cannot be negative.");

            double area = Math.PI * radius * radius;
            double circumference = 2 * Math.PI * radius;

            Console.WriteLine($"Area: {area}");
            Console.WriteLine($"Circumference: {circumference}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error extracting radius: {ex.Message}");
        }
    }
}
