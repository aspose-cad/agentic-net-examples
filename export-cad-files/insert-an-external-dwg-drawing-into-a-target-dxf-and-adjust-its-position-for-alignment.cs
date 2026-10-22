using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        const string targetPath = "target.dxf";
        const string sourcePath = "source.dwg";
        const string outputPath = "merged_output.dxf";

        if (!File.Exists(targetPath))
        {
            Console.Error.WriteLine($"Target file not found: {targetPath}");
            return;
        }

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the target DXF file
            using (Image targetImg = Image.Load(targetPath))
            {
                CadImage targetImage = (CadImage)targetImg;

                // Load the external DWG drawing to be inserted
                using (Image sourceImg = Image.Load(sourcePath))
                {
                    CadImage sourceImage = (CadImage)sourceImg;

                    // Create a new block entity to hold the source drawing entities
                    CadBlockEntity block = new CadBlockEntity
                    {
                        Name = "InsertedBlock"
                    };

                    // Add each entity from the source drawing into the block
                    foreach (CadEntityBase entity in sourceImage.Entities)
                    {
                        // NOTE: Position adjustment (e.g., translation) is not demonstrated
                        // because the Aspose.CAD API does not expose a direct transformation matrix.
                        block.AddEntity(entity);
                    }

                    // Add the block to the target drawing's block dictionary
                    // The CadBlockDictionary supports adding by name.
                    targetImage.BlockEntities.Add(block.Name, block);

                    // NOTE: Inserting the block into the drawing at a specific location would
                    // require an Insert entity (e.g., CadInsertEntity) which is not exposed
                    // by the current Aspose.CAD API. Therefore, the block is added to the
                    // drawing but not placed at a specific coordinate.
                }

                // Prepare DXF save options to preserve editable objects (Convert mode)
                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary,
                    Version = DxfOutputVersion.R12
                };

                // Save the modified drawing as DXF
                targetImage.Save(outputPath, dxfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}