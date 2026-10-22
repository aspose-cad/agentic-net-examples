using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace InsertDwginDxf
{
    class Program
    {
        static void Main(string[] args)
        {
            // Paths to source DWG and target DXF files
            string dwgPath = "source.dwg";
            string dxfPath = "target.dxf";
            string outputPath = "merged_output.dxf";

            // Verify source files exist
            if (!File.Exists(dwgPath))
            {
                Console.WriteLine($"DWG source file not found: {dwgPath}");
                return;
            }

            if (!File.Exists(dxfPath))
            {
                Console.WriteLine($"DXF target file not found: {dxfPath}");
                return;
            }

            try
            {
                // Load the DWG drawing
                using (Image dwgImage = Image.Load(dwgPath))
                {
                    CadImage dwgCad = dwgImage as CadImage;
                    if (dwgCad == null)
                    {
                        Console.WriteLine("Failed to cast DWG image to CadImage.");
                        return;
                    }

                    // Load the DXF that already contains a background image
                    using (Image dxfImage = Image.Load(dxfPath))
                    {
                        CadImage dxfCad = dxfImage as CadImage;
                        if (dxfCad == null)
                        {
                            Console.WriteLine("Failed to cast DXF image to CadImage.");
                            return;
                        }

                        // Get the Model Space block where entities are stored
                        if (!dxfCad.BlockEntities.TryGetValue("*Model_Space", out CadBlockEntity modelSpace))
                        {
                            Console.WriteLine("Model space block not found in DXF.");
                            return;
                        }

                        // Insert all entities from the DWG into the DXF's model space
                        foreach (var dwgEntity in dwgCad.Entities)
                        {
                            modelSpace.Entities.Add(dwgEntity);
                        }

                        // Save the merged drawing as DXF
                        try
                        {
                            DxfOptions dxfOptions = new DxfOptions();
                            dxfCad.Save(outputPath, dxfOptions);
                            Console.WriteLine("DWG inserted into DXF and saved to: " + outputPath);
                        }
                        catch (Exception saveEx)
                        {
                            Console.WriteLine("Error saving merged DXF: " + saveEx.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred during processing: " + ex.Message);
            }
        }
    }
}
