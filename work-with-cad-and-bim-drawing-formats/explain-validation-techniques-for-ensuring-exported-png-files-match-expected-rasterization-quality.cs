using System;
using System.IO;
using System.Security.Cryptography;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD file and output PNG file paths
            string cadPath = "input.dwg";
            string pngPath = "output.png";

            // Verify input CAD file exists
            if (!File.Exists(cadPath))
            {
                Console.WriteLine($"Input CAD file not found: {cadPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(pngPath);
            if (!string.IsNullOrEmpty(outputDir))
                Directory.CreateDirectory(outputDir);

            // Prepare rasterization options with high quality settings
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                UnitType = UnitType.Centimenter,
                PageWidth = 800,   // desired width in pixels
                PageHeight = 600,  // desired height in pixels
                AutomaticLayoutsScaling = true,
                Quality = new RasterizationQuality
                {
                    Text = RasterizationQualityValue.High,
                    Hatch = RasterizationQualityValue.High,
                    Arc = RasterizationQualityValue.High
                }
            };

            // Load the CAD image, apply raster options, and export to PNG
            using (Image cadImage = Image.Load(cadPath))
            {
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                cadImage.Save(pngPath, pngOptions);
            }

            // -------------------- Validation Section --------------------
            // 1. Verify that the PNG file was created
            if (!File.Exists(pngPath))
            {
                Console.WriteLine("Export failed: PNG file not found.");
                return;
            }

            // 2. Load the PNG header to check dimensions
            if (TryGetPngDimensions(pngPath, out int actualWidth, out int actualHeight))
            {
                int expectedWidth = (int)rasterOptions.PageWidth;
                int expectedHeight = (int)rasterOptions.PageHeight;

                Console.WriteLine($"Exported PNG size: {actualWidth}x{actualHeight}");
                if (actualWidth != expectedWidth || actualHeight != expectedHeight)
                {
                    Console.WriteLine("Dimension mismatch: exported image does not match expected raster size.");
                }
                else
                {
                    Console.WriteLine("Dimensions match the expected raster size.");
                }
            }
            else
            {
                Console.WriteLine("Unable to read PNG dimensions for validation.");
            }

            // 3. Optional: Compute a SHA‑256 hash of the PNG and compare with a known good hash.
            // This helps ensure pixel‑level fidelity.
            string referenceHash = "PLACEHOLDER_FOR_KNOWN_GOOD_HASH"; // replace with actual hash if available
            string actualHash = ComputeFileHash(pngPath);
            Console.WriteLine($"Computed SHA‑256 hash: {actualHash}");

            if (referenceHash != "PLACEHOLDER_FOR_KNOWN_GOOD_HASH" &&
                !string.Equals(actualHash, referenceHash, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Hash mismatch: PNG content differs from the reference file.");
            }
            else
            {
                Console.WriteLine("Hash matches the reference (or reference not provided).");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    // Helper method to compute SHA‑256 hash of a file
    private static string ComputeFileHash(string filePath)
    {
        using (FileStream stream = File.OpenRead(filePath))
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }

    // Reads PNG header to extract width and height (returns false if parsing fails)
    private static bool TryGetPngDimensions(string filePath, out int width, out int height)
    {
        width = height = 0;
        try
        {
            using (FileStream fs = File.OpenRead(filePath))
            using (BinaryReader br = new BinaryReader(fs))
            {
                // PNG signature (8 bytes)
                byte[] signature = br.ReadBytes(8);
                if (signature.Length != 8 ||
                    signature[0] != 0x89 || signature[1] != 0x50 || signature[2] != 0x4E ||
                    signature[3] != 0x47 || signature[4] != 0x0D || signature[5] != 0x0A ||
                    signature[6] != 0x1A || signature[7] != 0x0A)
                {
                    return false;
                }

                // Read chunks until IHDR is found
                while (fs.Position < fs.Length)
                {
                    uint length = ReadBigEndianUInt32(br);
                    string chunkType = new string(br.ReadChars(4));

                    if (chunkType == "IHDR")
                    {
                        width = (int)ReadBigEndianUInt32(br);
                        height = (int)ReadBigEndianUInt32(br);
                        return true;
                    }
                    else
                    {
                        // Skip this chunk (data + CRC)
                        fs.Seek(length + 4, System.IO.SeekOrigin.Current);
                    }
                }
            }
        }
        catch
        {
            // ignore parsing errors
        }
        return false;
    }

    private static uint ReadBigEndianUInt32(BinaryReader br)
    {
        byte[] bytes = br.ReadBytes(4);
        if (bytes.Length < 4) return 0;
        return ((uint)bytes[0] << 24) |
               ((uint)bytes[1] << 16) |
               ((uint)bytes[2] << 8) |
               bytes[3];
    }
}