using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadCircleMetrics
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder input CAD file path – replace with an actual file when testing.
            string inputPath = "input.dwg";

            // Define a local output directory for any generated files.
            string outputDirectory = "output";

            // Verify that the input file exists before proceeding.
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists; create it if it does not.
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Path for the text file that will contain the computed metrics.
            string metricsFilePath = Path.Combine(outputDirectory, "CircleMetrics.txt");

            // Wrap the entire processing block in a try/catch to handle unexpected errors.
            try
            {
                // Load the CAD file using Aspose.CAD.Image.Load.
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the generic Image to a CadImage to gain access to CAD‑specific members.
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("The loaded file is not a recognized CAD image.");
                        return;
                    }

                    // Open a StreamWriter to record the results in a text file.
                    using (StreamWriter writer = new StreamWriter(metricsFilePath, false))
                    {
                        int totalCircles = 0;

                        // -----------------------------------------------------------------
                        // 1. Process top‑level entities directly contained in the CAD drawing.
                        // -----------------------------------------------------------------
                        foreach (CadEntityBase entity in cadImage.Entities)
                        {
                            // Handle arcs first because CadArc derives from CadCircle.
                            if (entity is CadArc arc)
                            {
                                double radius = arc.Radius;
                                double area = Math.PI * radius * radius;
                                double circumference = 2 * Math.PI * radius;
                                string line = $"Arc – Center: ({arc.CenterPoint.X:F3}, {arc.CenterPoint.Y:F3}), Radius: {radius:F3}, Area: {area:F3}, Circumference: {circumference:F3}";
                                Console.WriteLine(line);
                                writer.WriteLine(line);
                                totalCircles++;
                            }
                            else if (entity is CadCircle circle)
                            {
                                double radius = circle.Radius;
                                double area = Math.PI * radius * radius;
                                double circumference = 2 * Math.PI * radius;
                                string line = $"Circle – Center: ({circle.CenterPoint.X:F3}, {circle.CenterPoint.Y:F3}), Radius: {radius:F3}, Area: {area:F3}, Circumference: {circumference:F3}";
                                Console.WriteLine(line);
                                writer.WriteLine(line);
                                totalCircles++;
                            }
                        }

                        // -----------------------------------------------------------------
                        // 2. Process circles that may reside inside block definitions.
                        // -----------------------------------------------------------------
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            // Each block can contain its own collection of entities.
                            foreach (CadEntityBase entity in block.Entities)
                            {
                                if (entity is CadArc arc)
                                {
                                    double radius = arc.Radius;
                                    double area = Math.PI * radius * radius;
                                    double circumference = 2 * Math.PI * radius;
                                    string line = $"Arc (Block: {block.Name}) – Center: ({arc.CenterPoint.X:F3}, {arc.CenterPoint.Y:F3}), Radius: {radius:F3}, Area: {area:F3}, Circumference: {circumference:F3}";
                                    Console.WriteLine(line);
                                    writer.WriteLine(line);
                                    totalCircles++;
                                }
                                else if (entity is CadCircle circle)
                                {
                                    double radius = circle.Radius;
                                    double area = Math.PI * radius * radius;
                                    double circumference = 2 * Math.PI * radius;
                                    string line = $"Circle (Block: {block.Name}) – Center: ({circle.CenterPoint.X:F3}, {circle.CenterPoint.Y:F3}), Radius: {radius:F3}, Area: {area:F3}, Circumference: {circumference:F3}";
                                    Console.WriteLine(line);
                                    writer.WriteLine(line);
                                    totalCircles++;
                                }
                            }
                        }

                        // Summarize the total number of circles (including arcs) found.
                        string summary = $"Total circles (including arcs) discovered: {totalCircles}";
                        Console.WriteLine(summary);
                        writer.WriteLine(summary);
                    } // End of StreamWriter using block.
                } // End of Image using block.
            }
            catch (Exception ex)
            {
                // Output a concise error message to the error stream.
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}