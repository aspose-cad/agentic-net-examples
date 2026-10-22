using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace CadLayoutDimensions
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input CAD file path (replace with actual file)
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD drawing
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Get the first layout (if any)
                    var firstLayout = cadImage.Layouts.Values.FirstOrDefault();
                    if (firstLayout == null)
                    {
                        Console.WriteLine("No layouts found in the drawing.");
                        return;
                    }

                    // Use dynamic to access width/height properties that may vary between SDK versions
                    dynamic layoutDyn = firstLayout;
                    float layoutWidth = 0;
                    float layoutHeight = 0;

                    try
                    {
                        layoutWidth = (float)layoutDyn.Width;
                        layoutHeight = (float)layoutDyn.Height;
                    }
                    catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                    {
                        // Fallback to alternative property names if the primary ones are unavailable
                        try
                        {
                            layoutWidth = (float)layoutDyn.WidthInUnits;
                            layoutHeight = (float)layoutDyn.HeightInUnits;
                        }
                        catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                        {
                            Console.WriteLine("Unable to determine layout dimensions: properties not found.");
                            return;
                        }
                    }

                    Console.WriteLine($"First layout dimensions: Width = {layoutWidth}, Height = {layoutHeight}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}