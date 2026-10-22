using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                string inputPath = "input.dwg";
                string outputPath = "output.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file does not exist: {inputPath}");
                    return;
                }

                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    CadMText mText = new CadMText();
                    mText.Text = "Sample MText";
                    mText.InsertionPoint = new Cad3DPoint(0, 0, 0);
                    mText.InitialTextHeight = 2.5;
                    mText.RotationAngleRad = 0;

                    CadBlockEntity modelSpace = cadImage.BlockEntities["*MODEL_SPACE"];
                    modelSpace.AddEntity(mText);

                    cadImage.Save(outputPath);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}