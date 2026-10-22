using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "InputFiles/sample.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "Output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            string outputPath = Path.Combine(outputDir, "sample.jpg");

            int expectedWidth = 800;
            int expectedHeight = 600;

            using (Image cadImage = Image.Load(inputPath))
            {
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = expectedWidth,
                    PageHeight = expectedHeight,
                    Layouts = new string[] { "Model" }
                };

                JpegOptions jpegOptions = new JpegOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, jpegOptions);
            }

            if (TryGetJpegDimensions(outputPath, out int actualWidth, out int actualHeight))
            {
                if (actualWidth == expectedWidth && actualHeight == expectedHeight)
                {
                    Console.WriteLine("JPEG resolution validation passed.");
                }
                else
                {
                    Console.Error.WriteLine($"JPEG resolution validation failed. Expected: {expectedWidth}x{expectedHeight}, Actual: {actualWidth}x{actualHeight}");
                }
            }
            else
            {
                Console.Error.WriteLine("Failed to read JPEG dimensions.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static bool TryGetJpegDimensions(string filePath, out int width, out int height)
    {
        width = 0;
        height = 0;
        try
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                if (br.ReadByte() != 0xFF || br.ReadByte() != 0xD8)
                    return false; // Not a JPEG

                while (fs.Position < fs.Length)
                {
                    // Find marker start 0xFF
                    byte b = br.ReadByte();
                    while (b != 0xFF && fs.Position < fs.Length)
                        b = br.ReadByte();

                    // Skip any padding FF bytes
                    byte marker = br.ReadByte();
                    while (marker == 0xFF)
                        marker = br.ReadByte();

                    // SOF0 (0xC0) or SOF2 (0xC2) contain dimensions
                    if (marker == 0xC0 || marker == 0xC2)
                    {
                        ushort length = ReadBigEndianUInt16(br);
                        br.ReadByte(); // precision
                        height = ReadBigEndianUInt16(br);
                        width = ReadBigEndianUInt16(br);
                        return true;
                    }
                    else
                    {
                        ushort length = ReadBigEndianUInt16(br);
                        if (length < 2)
                            return false;
                        fs.Seek((long)length - 2, System.IO.SeekOrigin.Current);
                    }
                }
            }
        }
        catch
        {
            return false;
        }
        return false;
    }

    static ushort ReadBigEndianUInt16(BinaryReader br)
    {
        byte high = br.ReadByte();
        byte low = br.ReadByte();
        return (ushort)((high << 8) | low);
    }
}