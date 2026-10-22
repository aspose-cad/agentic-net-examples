using System;
using System.IO;

namespace AsposeCadCircleExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Prepare output directory and file path
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, "circle.dxf");

                // Write a minimal DXF file containing a single CIRCLE entity
                using (StreamWriter writer = new StreamWriter(outputPath))
                {
                    // Header section (empty)
                    writer.WriteLine("0");
                    writer.WriteLine("SECTION");
                    writer.WriteLine("2");
                    writer.WriteLine("HEADER");
                    writer.WriteLine("0");
                    writer.WriteLine("ENDSEC");

                    // Entities section
                    writer.WriteLine("0");
                    writer.WriteLine("SECTION");
                    writer.WriteLine("2");
                    writer.WriteLine("ENTITIES");

                    // Circle entity definition
                    writer.WriteLine("0");
                    writer.WriteLine("CIRCLE");
                    writer.WriteLine("8");   // Layer name
                    writer.WriteLine("0");
                    writer.WriteLine("10");  // Center X
                    writer.WriteLine("0");
                    writer.WriteLine("20");  // Center Y
                    writer.WriteLine("0");
                    writer.WriteLine("30");  // Center Z
                    writer.WriteLine("0");
                    writer.WriteLine("40");  // Radius
                    writer.WriteLine("50");

                    // End of entities section
                    writer.WriteLine("0");
                    writer.WriteLine("ENDSEC");

                    // End of file
                    writer.WriteLine("0");
                    writer.WriteLine("EOF");
                }

                Console.WriteLine($"DXF file with circle saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}