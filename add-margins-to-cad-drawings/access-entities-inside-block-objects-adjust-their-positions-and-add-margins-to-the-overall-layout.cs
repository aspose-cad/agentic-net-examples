using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

namespace AddMarginsToCadDrawing
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input and output file paths
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPath = "output.png";

                    // Margin values to add (in drawing units)
                    double marginX = 10.0;
                    double marginY = 10.0;

                    // Load the CAD drawing
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Adjust positions of block entities
                        foreach (CadBlockEntity blockEntity in cadImage.BlockEntities.ValuesTyped)
                        {
                            // Shift the block's base point
                            if (blockEntity.BasePoint is Cad2DPoint basePt)
                            {
                                basePt.X += marginX;
                                basePt.Y += marginY;
                            }

                            // Iterate over entities inside the block
                            foreach (object entityObj in blockEntity.Entities)
                            {
                                // Handle inserted block objects
                                if (entityObj is CadInsertObject insertObj)
                                {
                                    if (insertObj.InsertionPoint is Cad2DPoint insertPt)
                                    {
                                        insertPt.X += marginX;
                                        insertPt.Y += marginY;
                                    }
                                }
                                // Handle simple line entities
                                else if (entityObj is CadLine line)
                                {
                                    if (line.FirstPoint is Cad2DPoint firstPt)
                                    {
                                        firstPt.X += marginX;
                                        firstPt.Y += marginY;
                                    }

                                    if (line.SecondPoint is Cad2DPoint secondPt)
                                    {
                                        secondPt.X += marginX;
                                        secondPt.Y += marginY;
                                    }
                                }
                                // Additional entity types can be processed here similarly
                            }
                        }

                        // Save the modified drawing as PNG
                        PngOptions pngOptions = new PngOptions();
                        cadImage.Save(outputPath, pngOptions);
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