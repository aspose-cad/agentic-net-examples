using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.UnderlayDefinition;

class ExportDgnUnderlay
{
    static void Main()
    {
        const string dxfPath = "drawing.dxf";
        const string outputPdfPath = "underlay.pdf";

        if (!File.Exists(dxfPath))
        {
            Console.WriteLine($"Input DXF file not found: {dxfPath}");
            return;
        }

        try
        {
            // Load the DXF drawing that contains the embedded DGN underlay
            using (CadImage dxfImage = (CadImage)Image.Load(dxfPath))
            {
                // Locate the underlay definition object within the DXF file
                CadUnderlayDefinition underlayDef = dxfImage.Objects
                    .FirstOrDefault(o => o is CadUnderlayDefinition) as CadUnderlayDefinition;

                if (underlayDef == null)
                {
                    Console.WriteLine("No DGN underlay found in the DXF file.");
                    return;
                }

                // Retrieve the path (or file name) of the embedded DGN underlay
                string dgnPath = underlayDef.UnderlayPathAndFileName;

                if (string.IsNullOrWhiteSpace(dgnPath) || !File.Exists(dgnPath))
                {
                    Console.WriteLine($"Embedded DGN underlay file not found: {dgnPath}");
                    return;
                }

                // Load the DGN underlay using Aspose.CAD
                using (Image dgnImage = Image.Load(dgnPath))
                {
                    // Prepare PDF export options
                    var pdfOptions = new PdfOptions();

                    // Ensure output directory exists
                    string outputDir = Path.GetDirectoryName(outputPdfPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Export the DGN underlay to a separate PDF file
                    dgnImage.Save(outputPdfPath, pdfOptions);
                }
            }

            Console.WriteLine($"DGN underlay exported to {outputPdfPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}