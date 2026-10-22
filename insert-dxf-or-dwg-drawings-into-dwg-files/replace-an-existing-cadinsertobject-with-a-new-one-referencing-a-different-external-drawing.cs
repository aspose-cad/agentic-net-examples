using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class ReplaceInsertObject
{
    static void Main()
    {
        // Input and output file paths (replace with actual paths)
        string inputPath = @"C:\CAD\input.dwg";
        string outputPath = @"C:\CAD\output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);

        try
        {
            // Load the CAD document
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Find the first CadInsertObject in the drawing
                CadInsertObject insertObject = null;
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadInsertObject insert)
                    {
                        insertObject = insert;
                        break;
                    }
                }

                if (insertObject != null)
                {
                    // Replace the external drawing reference
                    insertObject.Name = "NewExternalDrawing.dwg";
                    // Other properties (InsertionPoint, ScaleX/Y/Z, RotationAngle, etc.) remain unchanged
                }
                else
                {
                    Console.WriteLine("No CadInsertObject found in the drawing.");
                }

                // Save the modified drawing (format inferred from file extension)
                cadImage.Save(outputPath);
            }

            Console.WriteLine("Drawing saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}