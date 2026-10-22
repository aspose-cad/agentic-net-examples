using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the large CAD drawing
            string inputPath = "large_drawing.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to work with CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Count text entities before changing the default font
                int textEntityCount = cadImage.Entities.Count(e => e is CadMText || e is CadText);
                Console.WriteLine($"Number of text entities: {textEntityCount}");

                // Measure the time required to set the DefaultFont property
                Stopwatch sw = Stopwatch.StartNew();
                cadImage.DefaultFont = "Arial";
                sw.Stop();
                Console.WriteLine($"Time to set DefaultFont: {sw.ElapsedMilliseconds} ms");

                // Enumerate text entities after the change to observe any additional processing cost
                sw.Restart();
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadMText mtext)
                    {
                        // Example access: just read the text content
                        string textContent = mtext.Text;
                    }
                    else if (entity is CadText text)
                    {
                        // Example access: just read the default value
                        string textContent = text.DefaultValue;
                    }
                }
                sw.Stop();
                Console.WriteLine($"Time to enumerate text entities after DefaultFont change: {sw.ElapsedMilliseconds} ms");

                // Save the modified drawing while preserving editable CAD objects
                string outputPath = "large_drawing_modified.dxf";
                DxfOptions dxfOptions = new DxfOptions { OutputMode = CadOutputMode.Convert };
                cadImage.Save(outputPath, dxfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}