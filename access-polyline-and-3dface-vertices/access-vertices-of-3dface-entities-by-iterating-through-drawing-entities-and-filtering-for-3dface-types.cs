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

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Iterate through top‑level drawing entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Filter for 3DFace entities
                    if (entity is Cad3DFace face)
                    {
                        // Access the four corner points of the 3DFace
                        Cad3DPoint firstCorner = face.FirstCorner;
                        Cad3DPoint secondCorner = face.SecondCorner;
                        Cad3DPoint thirdCorner = face.ThirdCorner;
                        Cad3DPoint fourthCorner = face.FourthCorner;

                        // Output the corner coordinates
                        Console.WriteLine(
                            $"3DFace corners: " +
                            $"({firstCorner.X}, {firstCorner.Y}, {firstCorner.Z}) " +
                            $"({secondCorner.X}, {secondCorner.Y}, {secondCorner.Z}) " +
                            $"({thirdCorner.X}, {thirdCorner.Y}, {thirdCorner.Z}) " +
                            $"({fourthCorner.X}, {fourthCorner.Y}, {fourthCorner.Z})");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}