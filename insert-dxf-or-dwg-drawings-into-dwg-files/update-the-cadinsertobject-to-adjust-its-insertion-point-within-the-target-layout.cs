using System;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Load the existing CAD file
                string inputPath = "input.dxf";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Define the new insertion point
                Cad3DPoint newInsertionPoint = new Cad3DPoint(100.0, 200.0, 0.0);

                // Update insertion point for each CadInsertObject in the image
                CadInsertObject[] updatedInserts = cadImage.Entities
                    .OfType<CadInsertObject>()
                    .Select(insert =>
                    {
                        insert.InsertionPoint = newInsertionPoint;
                        return insert;
                    })
                    .ToArray();

                // Preserve other entities that are not CadInsertObject
                CadEntityBase[] otherEntities = cadImage.Entities
                    .Where(entity => !(entity is CadInsertObject))
                    .ToArray();

                // Replace the Entities collection with the updated set
                cadImage.Entities = otherEntities.Concat(updatedInserts).ToArray();

                // Prepare DXF save options to preserve editable objects
                DxfOptions saveOptions = new DxfOptions();
                saveOptions.OutputMode = CadOutputMode.Convert;
                saveOptions.DxfFileFormat = CadFileFormat.Binary;
                saveOptions.Version = DxfOutputVersion.R12;

                // Save the modified CAD file
                string outputPath = "output.dxf";
                cadImage.Save(outputPath, saveOptions);
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}