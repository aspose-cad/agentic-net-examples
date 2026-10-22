using System;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Create a new CAD image instance
        using (var cadImage = new CadImage())
        {
            // Define four corner points of the 3DFACE
            var point1 = new Cad3DPoint(0, 0, 0);
            var point2 = new Cad3DPoint(100, 0, 0);
            var point3 = new Cad3DPoint(100, 100, 0);
            var point4 = new Cad3DPoint(0, 100, 0);

            // Create the 3DFACE entity
            var threeDFace = new Cad3DFace(point1, point2, point3, point4);

            // Retrieve the model space block (first block) and add the 3DFACE entity
            var modelSpaceBlock = cadImage.BlockEntities.ValuesTyped.FirstOrDefault();
            if (modelSpaceBlock != null)
            {
                modelSpaceBlock.AddEntity(threeDFace);
            }
            else
            {
                Console.WriteLine("Model space block not found.");
                return;
            }

            // Save the CAD image as a DWG file with error handling
            try
            {
                var dwgOptions = new DwgOptions();
                cadImage.Save("output.dwg", dwgOptions);
                Console.WriteLine("DWG file saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving DWG file: {ex.Message}");
            }
        }
    }
}