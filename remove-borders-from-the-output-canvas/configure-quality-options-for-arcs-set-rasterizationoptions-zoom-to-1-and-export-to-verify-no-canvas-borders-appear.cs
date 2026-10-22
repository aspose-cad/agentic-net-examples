using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input CAD file path (adjust as needed)
                    string inputFilePath = "sample.dwg";

                    // Verify that the input file exists
                    if (!File.Exists(inputFilePath))
                    {
                        Console.WriteLine($"Input file '{inputFilePath}' not found. Please provide a valid CAD drawing.");
                        return;
                    }

                    // Create output directory
                    string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                    Directory.CreateDirectory(outputDir);

                    // Load the CAD drawing
                    using (Image image = Image.Load(inputFilePath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // ------------------------------
                        // Export to PDF (vector output)
                        // ------------------------------
                        PdfOptions pdfOptions = new PdfOptions();
                        CadRasterizationOptions pdfRasterOptions = new CadRasterizationOptions
                        {
                            // Example: select a specific layout if needed
                            Layouts = new string[] { "Model" },
                            // Example: select specific layers if needed
                            Layers = new string[] { "0" },
                            // Ensure no extra borders
                            Zoom = 1
                        };
                        pdfOptions.VectorRasterizationOptions = pdfRasterOptions;

                        string pdfOutputPath = Path.Combine(outputDir, "output.pdf");
                        cadImage.Save(pdfOutputPath, pdfOptions);
                        Console.WriteLine($"PDF exported to: {pdfOutputPath}");

                        // ------------------------------
                        // Export to SVG (vector output)
                        // ------------------------------
                        SvgOptions svgOptions = new SvgOptions();
                        CadRasterizationOptions svgRasterOptions = new CadRasterizationOptions
                        {
                            Layouts = new string[] { "Model" },
                            Layers = new string[] { "0" },
                            Zoom = 1
                        };
                        svgOptions.VectorRasterizationOptions = svgRasterOptions;

                        string svgOutputPath = Path.Combine(outputDir, "output.svg");
                        cadImage.Save(svgOutputPath, svgOptions);
                        Console.WriteLine($"SVG exported to: {svgOutputPath}");

                        // ------------------------------
                        // Inspect block entities
                        // ------------------------------
                        Console.WriteLine("Block entities in the drawing:");
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            Console.WriteLine($"Block Name: {block.Name}");
                            foreach (CadEntityBase entity in block.Entities)
                            {
                                string entityType = entity.GetType().Name;
                                Console.WriteLine($"  Entity Type: {entityType}, Layer: {entity.LayerName}");
                            }
                        }

                        // ------------------------------
                        // Inspect top-level drawing entities
                        // ------------------------------
                        Console.WriteLine("Top-level drawing entities:");
                        foreach (CadEntityBase entity in cadImage.Entities)
                        {
                            string entityType = entity.GetType().Name;
                            Console.WriteLine($"Entity Type: {entityType}, Layer: {entity.LayerName}");
                        }
                    }
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}