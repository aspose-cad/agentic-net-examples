using System;
using System.Diagnostics;
using System.IO;
using Aspose.CAD;

namespace SvgLoadPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string svgPath = "sample.svg";

                // Create a minimal SVG file if it does not exist
                if (!File.Exists(svgPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(svgPath)) ?? ".");
                    string minimalSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""100"" height=""100"">
  <rect width=""100"" height=""100"" fill=""red"" />
</svg>";
                    File.WriteAllText(svgPath, minimalSvg);
                }

                // Measure loading SVG using Image.Load with a file path
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Image imageFromPath = Image.Load(svgPath);
                stopwatch.Stop();
                Console.WriteLine($"Load via Image.Load (file path): {stopwatch.ElapsedMilliseconds} ms");
                imageFromPath.Dispose();

                // Measure loading SVG using Image.Load with a stream
                using (FileStream fileStream = new FileStream(svgPath, FileMode.Open, FileAccess.Read))
                {
                    stopwatch.Restart();
                    Image imageFromStream = Image.Load(fileStream);
                    stopwatch.Stop();
                    Console.WriteLine($"Load via Image.Load (stream): {stopwatch.ElapsedMilliseconds} ms");
                    imageFromStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}