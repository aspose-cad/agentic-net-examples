using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder paths – replace with actual file locations
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD file
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Set a default font that will be used when the original font cannot be found
                cadImage.DefaultFont = "Arial";

                // Save the CAD drawing to PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);

                // Log the default font used for substitution
                Console.WriteLine($"Default font used for substitution: {cadImage.DefaultFont}");

                // Iterate through all layouts and their entities to log text entities
                foreach (KeyValuePair<string, CadLayout> layoutPair in cadImage.Layouts)
                {
                    CadLayout layout = layoutPair.Value;
                    foreach (CadEntityBase entity in layout.Entities)
                    {
                        if (entity.TypeName == CadEntityTypeName.TEXT)
                        {
                            // Since font information is not exposed, we assume substitution may have occurred
                            Console.WriteLine($"Layout \"{layoutPair.Key}\": Text entity (Handle: {entity.ObjectHandle}) may have used default font \"{cadImage.DefaultFont}\" for substitution.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}