using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "TextExample.dxf");

            using (CadImage cadImage = new CadImage())
            {
                // Get the first layout (usually ModelSpace)
                var layout = cadImage.Layouts.Values.FirstOrDefault();
                if (layout == null)
                    throw new InvalidOperationException("No layout found in the CAD image.");

                // Create a TEXT entity
                CadText textEntity = new CadText
                {
                    DefaultValue = "Hello Aspose.CAD",
                    FirstAlignment = new Cad3DPoint(10, 20, 0),
                    TextHeight = 5,
                    SoftOwner = layout.BlockTableRecordHandle
                };

                // Add the TEXT entity to the drawing
                cadImage.Entities = cadImage.Entities
                    .Concat(new CadEntityBase[] { textEntity })
                    .ToArray();

                // Save the drawing as DXF
                DxfOptions options = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath, options);
            }

            Console.WriteLine($"DXF file saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}