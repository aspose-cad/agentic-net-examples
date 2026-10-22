using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.UnderlayDefinition;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD file
            var cadImage = (CadImage)Image.Load(inputPath);

            // Scan all entities for DGN underlay references
            foreach (var entity in cadImage.Entities)
            {
                if (entity.TypeName == CadEntityTypeName.DGNUNDERLAY)
                {
                    var dgnUnderlay = (CadDgnUnderlay)entity;

                    var underlayDef = cadImage.Objects
                        .OfType<CadUnderlayDefinition>()
                        .FirstOrDefault(def => def.ObjectHandle == dgnUnderlay.AcDbUnderlayDefinitionObjectId);

                    string underlayPath = underlayDef != null
                        ? underlayDef.UnderlayPathAndFileName
                        : "Definition not found";

                    Console.WriteLine($"DGN Underlay found: Name = {dgnUnderlay.UnderlayName}, Path = {underlayPath}");
                }
            }

            // Ensure output directory exists
            var outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the CAD file preserving editable objects (optional)
            var saveOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                DxfFileFormat = CadFileFormat.Binary
            };
            cadImage.Save(outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}