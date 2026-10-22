using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadTextSearchExample
{
    class Program
    {
        static void Main()
        {
            string inputPath = "sample.dwg";
            string searchPhrase = "SampleText";

            try
            {
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Loaded file is not a CAD image.");
                        return;
                    }

                    List<Cad3DPoint> occurrencePoints = new List<Cad3DPoint>();

                    foreach (var entity in cadImage.Entities)
                    {
                        if (entity is CadText textEntity)
                        {
                            if (!string.IsNullOrEmpty(textEntity.DefaultValue) &&
                                textEntity.DefaultValue.IndexOf(searchPhrase, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                occurrencePoints.Add(textEntity.FirstAlignment);
                            }
                        }
                        else if (entity is CadMText mTextEntity)
                        {
                            if (!string.IsNullOrEmpty(mTextEntity.Text) &&
                                mTextEntity.Text.IndexOf(searchPhrase, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                occurrencePoints.Add(mTextEntity.InsertionPoint);
                            }
                        }
                    }

                    Console.WriteLine($"Found {occurrencePoints.Count} occurrence(s) of \"{searchPhrase}\":");
                    foreach (var point in occurrencePoints)
                    {
                        Console.WriteLine($"X: {point.X}, Y: {point.Y}, Z: {point.Z}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}