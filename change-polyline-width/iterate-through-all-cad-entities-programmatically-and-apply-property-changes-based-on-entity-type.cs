using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class CadEntityProcessor
{
    static void Main()
    {
        // Input and output paths (replace with actual paths)
        string inputPath = @"C:\CAD\input.dwg";
        string outputPath = @"C:\CAD\output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Process top‑level entities
                ProcessEntityCollection(cadImage.Entities);

                // Process entities inside each block
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    ProcessEntityCollection(block.Entities);
                }

                // Save the modified CAD file
                var saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath);
                Console.WriteLine($"CAD file saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }

    private static void ProcessEntityCollection(IEnumerable<CadEntityBase> entities)
    {
        foreach (CadEntityBase entity in entities)
        {
            switch (entity)
            {
                case CadLine line:
                    // Example placeholder for line modifications
                    break;

                case CadArc arc:
                    // Example placeholder for arc modifications
                    break;

                case CadCircle circle:
                    // Example placeholder for circle modifications
                    break;

                case CadLwPolyline lwPolyline:
                    // Set polyline to closed
                    lwPolyline.Flag = CadLwPolylineFlag.Closed;
                    // Placeholder for width change – not directly supported in current API
                    break;

                case CadPolyline polyline:
                    // Placeholder for polyline width change – not directly supported in current API
                    break;

                case CadText text:
                    // Append a note to existing text (DefaultValue holds the text content)
                    if (!string.IsNullOrEmpty(text.DefaultValue))
                    {
                        text.DefaultValue = $"{text.DefaultValue} [Modified]";
                    }
                    break;

                case CadMText mText:
                    // Append a note to MText
                    if (!string.IsNullOrEmpty(mText.Text))
                    {
                        mText.Text = $"{mText.Text} [Modified]";
                    }
                    break;

                default:
                    // No action for other entity types
                    break;
            }
        }
    }
}