using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source DXF file
            string sourcePath = "input.dxf";
            // Path to the output PDF file
            string outputPath = "output.pdf";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the DXF drawing
            using (Image image = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = (CadImage)image;

                // Define the name of the layer that contains the title block
                string titleBlockLayerName = "TitleBlock";

                // Determine the bounding box of the title block entities
                double minX = double.MaxValue;
                double minY = double.MaxValue;
                double maxX = double.MinValue;
                double maxY = double.MinValue;

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity.LayerName == titleBlockLayerName && entity.Bounds != null && entity.Bounds.Count > 0)
                    {
                        double entityMinX = entity.Bounds.Min(p => p.X);
                        double entityMinY = entity.Bounds.Min(p => p.Y);
                        double entityMaxX = entity.Bounds.Max(p => p.X);
                        double entityMaxY = entity.Bounds.Max(p => p.Y);

                        if (entityMinX < minX) minX = entityMinX;
                        if (entityMinY < minY) minY = entityMinY;
                        if (entityMaxX > maxX) maxX = entityMaxX;
                        if (entityMaxY > maxY) maxY = entityMaxY;
                    }
                }

                // If title block was found, compute translation to align its lower‑left corner to (0,0)
                if (minX != double.MaxValue && minY != double.MaxValue)
                {
                    double offsetX = -minX;
                    double offsetY = -minY;

                    // Apply translation to block insert objects on the title‑block layer
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity.LayerName == titleBlockLayerName && entity is CadInsertObject insertObj)
                        {
                            // Adjust the insertion point
                            insertObj.InsertionPoint = new Cad3DPoint(
                                insertObj.InsertionPoint.X + offsetX,
                                insertObj.InsertionPoint.Y + offsetY,
                                insertObj.InsertionPoint.Z);
                        }
                    }
                }

                // Save the modified drawing as PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}