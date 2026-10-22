using System;
using System.IO;
using Aspose.CAD;

class Program
{
    static void Main()
    {
        const string inputPath = "sample.dxb";

        // Ensure the input file exists; create a placeholder if missing
        if (!File.Exists(inputPath))
        {
            // Create an empty placeholder file (may not be a valid DXB)
            File.WriteAllBytes(inputPath, new byte[0]);
            Console.WriteLine($"Placeholder file created at '{inputPath}'. It may not be a valid DXB.");
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                int width = cadImage.Width;
                int height = cadImage.Height;

                Console.WriteLine($"Image dimensions: Width = {width} px, Height = {height} px");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}