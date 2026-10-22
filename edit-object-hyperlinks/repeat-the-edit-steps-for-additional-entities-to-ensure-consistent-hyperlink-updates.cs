using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Update hyperlinks in top‑level drawing entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadMText mtext && !string.IsNullOrEmpty(mtext.Text))
                    {
                        if (mtext.Text.Contains("http://old.com"))
                        {
                            mtext.Text = mtext.Text.Replace("http://old.com", "http://new.com");
                        }
                    }
                }

                // Update hyperlinks inside block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase blockEntity in block.Entities)
                    {
                        if (blockEntity is CadMText blockMText && !string.IsNullOrEmpty(blockMText.Text))
                        {
                            if (blockMText.Text.Contains("http://old.com"))
                            {
                                blockMText.Text = blockMText.Text.Replace("http://old.com", "http://new.com");
                            }
                        }
                    }
                }

                // Save the modified drawing
                DwgOptions saveOptions = new DwgOptions();
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}