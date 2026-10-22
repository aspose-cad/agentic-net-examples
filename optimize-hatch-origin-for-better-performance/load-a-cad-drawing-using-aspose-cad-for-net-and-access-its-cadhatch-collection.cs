using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        string inputFile = "sample.dxf";

        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file not found: {inputFile}");
            return;
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(inputFile))
            {
                foreach (var layout in cadImage.Layouts.Values)
                {
                    foreach (CadEntityBase entity in layout.Entities)
                    {
                        if (entity.TypeName == CadEntityTypeName.HATCH)
                        {
                            CadHatch hatch = (CadHatch)entity;

                            Console.WriteLine($"Hatch Handle: {hatch.ObjectHandle}");

                            if (hatch.ColorValue.HasValue)
                            {
                                Console.WriteLine($"True Color (ARGB): 0x{hatch.ColorValue.Value:X8}");
                            }
                            else
                            {
                                Console.WriteLine($"Indexed Color ID: {hatch.ColorId}");
                            }

                            Console.WriteLine($"Hatch Angle: {hatch.HatchAngle}");
                            Console.WriteLine($"Hatch Scale/Spacing: {hatch.HatchScaleOrSpacing}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}