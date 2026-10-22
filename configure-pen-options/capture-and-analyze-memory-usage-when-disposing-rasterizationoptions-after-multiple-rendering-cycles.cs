using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadMemoryAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file (DWG/DXF)
                string sourcePath = "sample.dwg";

                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Number of rendering cycles
                int cycles = 5;

                // Loop through rendering cycles and capture memory usage
                for (int i = 1; i <= cycles; i++)
                {
                    // Force garbage collection before measurement
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    long memoryBefore = GC.GetTotalMemory(forceFullCollection: true);

                    // Load CAD image
                    using (Image cadImage = Image.Load(sourcePath))
                    {
                        // Cast to CadImage to access CAD-specific members if needed
                        CadImage cad = cadImage as CadImage;

                        // Configure rasterization options
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            // Example: set page size
                            PageWidth = 1600,
                            PageHeight = 1200
                            // BackgroundColor omitted to avoid System.Drawing dependency
                        };

                        // Configure PNG output options and assign rasterization options
                        PngOptions pngOptions = new PngOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Save rendered image to a memory stream (no file I/O)
                        using (MemoryStream outputStream = new MemoryStream())
                        {
                            cadImage.Save(outputStream, pngOptions);
                            // Optionally, you could use outputStream.Length for further analysis
                        }

                        // Release rasterization options reference for GC
                        rasterOptions = null;
                    }

                    // Force garbage collection after disposing resources
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    long memoryAfter = GC.GetTotalMemory(forceFullCollection: true);
                    long memoryDelta = memoryAfter - memoryBefore;

                    Console.WriteLine($"Cycle {i}: Memory before = {memoryBefore:N0} bytes, after = {memoryAfter:N0} bytes, delta = {memoryDelta:N0} bytes");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}