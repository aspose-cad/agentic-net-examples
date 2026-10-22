using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputPath = "output.dwg";

            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                // Locate an existing CadText entity to use as a reference point
                CadText referenceText = cadImage.Entities.OfType<CadText>().FirstOrDefault();

                if (referenceText != null)
                {
                    // ----- Add a new CadText entity beside the existing one -----
                    var newText = new CadText
                    {
                        DefaultValue = "New Text",
                        FirstAlignment = new Cad3DPoint(
                            referenceText.FirstAlignment.X + 10,
                            referenceText.FirstAlignment.Y,
                            referenceText.FirstAlignment.Z),
                        TextHeight = referenceText.TextHeight
                    };
                    cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { newText }).ToArray();

                    // ----- Add a new CadMText entity beside the existing one -----
                    var newMText = new CadMText
                    {
                        Text = "New MText Content",
                        InsertionPoint = new Cad3DPoint(
                            referenceText.FirstAlignment.X,
                            referenceText.FirstAlignment.Y - 20,
                            referenceText.FirstAlignment.Z),
                        InitialTextHeight = referenceText.TextHeight * 2
                    };

                    // Ensure correct ownership within the model layout
                    var modelLayout = cadImage.Layouts?.Values?.FirstOrDefault();
                    newMText.SoftOwner = modelLayout?.BlockTableRecordHandle ?? string.Empty;

                    cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { newMText }).ToArray();
                }

                // Save the modified drawing back to DWG format
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}