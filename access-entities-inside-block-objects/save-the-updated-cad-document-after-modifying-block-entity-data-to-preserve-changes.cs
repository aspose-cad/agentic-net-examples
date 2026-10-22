using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Iterate through block definitions in the drawing
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    // Find the first line entity inside the block and modify its geometry
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        if (entity is CadLine line)
                        {
                            // Shift the start point 10 units along the X axis
                            Cad3DPoint start = line.FirstPoint;
                            Cad3DPoint newStart = new Cad3DPoint(start.X + 10, start.Y, start.Z);
                            line.FirstPoint = newStart;

                            // Shift the end point 10 units along the X axis
                            Cad3DPoint end = line.SecondPoint;
                            Cad3DPoint newEnd = new Cad3DPoint(end.X + 10, end.Y, end.Z);
                            line.SecondPoint = newEnd;

                            // Modify only the first line found in this block
                            break;
                        }
                    }
                }

                // Save the modified drawing to a new file
                try
                {
                    cadImage.Save(outputPath);
                    Console.WriteLine($"Modified drawing saved to: {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.WriteLine($"Error saving file: {saveEx.Message}");
                }
            }
        }
        catch (Exception loadEx)
        {
            Console.WriteLine($"Error loading file: {loadEx.Message}");
        }
    }
}