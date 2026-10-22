using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter the radius of the circle: ");
        string input = Console.ReadLine();

        if (double.TryParse(input, out double radius) && radius >= 0)
        {
            double area = Math.PI * radius * radius;
            double circumference = 2 * Math.PI * radius;

            Console.WriteLine($"Radius: {radius}");
            Console.WriteLine($"Area: {area:F2}");
            Console.WriteLine($"Circumference: {circumference:F2}");
        }
        else
        {
            Console.WriteLine("Invalid radius value.");
        }
    }
}
