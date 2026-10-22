using System.Linq;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // If you have a license, set it here
        // var license = new Aspose.CAD.License();
        // license.SetLicense("Aspose.CAD.lic");

        // Create an empty CAD image
        var cadImage = new CadImage();

        // Define the four corners of the 3DFACE
        var firstCorner = new Cad3DPoint(0, 0, 0);
        var secondCorner = new Cad3DPoint(100, 0, 0);
        var thirdCorner = new Cad3DPoint(100, 100, 0);
        var fourthCorner = new Cad3DPoint(0, 100, 0);

        // Create the 3DFACE entity
        var threeDFace = new Cad3DFace(firstCorner, secondCorner, thirdCorner, fourthCorner);

        // Add the 3DFACE entity to the image's entity collection
        var entityList = new List<CadEntityBase>(cadImage.Entities ?? Array.Empty<CadEntityBase>());
        entityList.Add(threeDFace);
        cadImage.Entities = entityList.ToArray();

        // Save the drawing as a DWG file with error handling
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