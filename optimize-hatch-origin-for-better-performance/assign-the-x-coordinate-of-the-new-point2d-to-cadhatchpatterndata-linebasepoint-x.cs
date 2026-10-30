using Aspose.CAD;
using System;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;

namespace OptimizeHatchOrigin
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Create a new CadHatchPatternData instance
                CadHatchPatternData hatchPatternData = new CadHatchPatternData();

                // Create a new Point2D and set its X coordinate
                Point2D newPoint = new Point2D
                {
                    X = 25.0
                };

                // Assign the X coordinate to the LineBasePoint.X of the hatch pattern data
                hatchPatternData.LineBasePoint.X = newPoint.X;

                // Output the result to verify
                Console.WriteLine($"LineBasePoint.X is now set to: {hatchPatternData.LineBasePoint.X}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}