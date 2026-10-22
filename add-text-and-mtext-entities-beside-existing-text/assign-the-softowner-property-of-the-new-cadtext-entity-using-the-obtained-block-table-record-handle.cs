using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the DWG file
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Obtain the Model Space block where most drawing entities reside
                CadBlockEntity modelSpace = cadImage.BlockEntities["*Model_Space"];

                // Find the layout that corresponds to Model Space to get its BlockTableRecordHandle
                string modelSpaceHandle = string.Empty;
                foreach (var layout in cadImage.Layouts.Values)
                {
                    if (layout.LayoutName == "*Model_Space")
                    {
                        modelSpaceHandle = layout.BlockTableRecordHandle;
                        break;
                    }
                }

                // Find an existing CadText entity in Model Space to use as a reference point
                CadText referenceText = null;
                foreach (CadEntityBase entity in modelSpace.Entities)
                {
                    if (entity is CadText txt)
                    {
                        referenceText = txt;
                        break;
                    }
                }

                if (referenceText != null && !string.IsNullOrEmpty(modelSpaceHandle))
                {
                    // Create a new CadText entity positioned beside the existing text
                    CadText newText = new CadText
                    {
                        DefaultValue = "Added Text",
                        FirstAlignment = new Cad3DPoint(
                            referenceText.FirstAlignment.X + 10,
                            referenceText.FirstAlignment.Y,
                            referenceText.FirstAlignment.Z),
                        TextHeight = referenceText.TextHeight,
                        SoftOwner = modelSpaceHandle
                    };

                    // Add the new CadText to Model Space
                    modelSpace.AddEntity(newText);

                    // Create a new CadMText entity positioned beside the existing text
                    CadMText newMText = new CadMText
                    {
                        Text = "Added MText",
                        InsertionPoint = new Cad3DPoint(
                            referenceText.FirstAlignment.X + 10,
                            referenceText.FirstAlignment.Y - 5,
                            referenceText.FirstAlignment.Z),
                        InitialTextHeight = referenceText.TextHeight,
                        SoftOwner = modelSpaceHandle
                    };

                    // Add the new CadMText to Model Space
                    modelSpace.AddEntity(newMText);
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}