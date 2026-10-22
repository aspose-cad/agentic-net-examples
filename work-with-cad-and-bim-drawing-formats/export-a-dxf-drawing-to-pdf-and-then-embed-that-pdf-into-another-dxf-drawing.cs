using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Paths for source DXF, intermediate PDF, target DXF and final combined DXF
            string sourceDxfPath = "source.dxf";
            string pdfPath = "source.pdf";
            string targetDxfPath = "target.dxf";
            string outputDxfPath = "combined.dxf";

            // Validate input files
            if (!File.Exists(sourceDxfPath))
            {
                Console.Error.WriteLine($"Source DXF not found: {sourceDxfPath}");
                return;
            }

            if (!File.Exists(targetDxfPath))
            {
                Console.Error.WriteLine($"Target DXF not found: {targetDxfPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputDxfPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the source DXF and export it to PDF
                using (Image sourceImage = Image.Load(sourceDxfPath))
                {
                    var pdfOptions = new PdfOptions();
                    sourceImage.Save(pdfPath, pdfOptions);
                }

                // Load the target DXF where the PDF would be embedded
                using (Image targetImage = Image.Load(targetDxfPath))
                {
                    var cadTarget = (CadImage)targetImage;

                    // NOTE: Aspose.CAD does not provide a supported way to embed a PDF file into a DXF drawing.
                    // The SDK supports underlay definitions for DWF/DWG, but PDF underlays are not available.
                    // Therefore, this example will simply copy the target DXF to the output path.

                    var dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,
                        Version = DxfOutputVersion.R12,
                        DxfFileFormat = CadFileFormat.Ascii
                    };

                    cadTarget.Save(outputDxfPath, dxfOptions);
                }

                Console.WriteLine("PDF export completed and target DXF saved to output path.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}