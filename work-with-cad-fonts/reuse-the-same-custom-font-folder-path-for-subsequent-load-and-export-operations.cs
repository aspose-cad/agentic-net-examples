using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace CadFontDemo
{
    class Program
    {
        static void Main()
        {
            // Define input and output locations.
            string inputFilePath = "input.dwg";                     // Placeholder path – replace with an actual DWG/DXF file.
            string outputDirectory = "output";                     // All generated files will be placed here.
            string outputFilePath = Path.Combine(outputDirectory, "output.dxf");

            // Verify that the input file exists before proceeding.
            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Error: The input file \"{inputFilePath}\" was not found.");
                return;
            }

            // Ensure the output directory exists; create it if necessary.
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error: Unable to create output directory \"{outputDirectory}\". {dirEx.Message}");
                return;
            }

            // Wrap the entire processing block in a try/catch to handle unexpected errors gracefully.
            try
            {
                // Load the CAD file using Aspose.CAD's generic loader.
                using (Image loadedImage = Image.Load(inputFilePath))
                {
                    // Cast the generic Image to a CadImage to gain access to CAD‑specific members.
                    CadImage cadImage = loadedImage as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Error: The loaded file could not be cast to a CadImage. Unsupported format?");
                        return;
                    }

                    // -----------------------------------------------------------------
                    // 1. Inspect the current default font used by the CAD document.
                    // -----------------------------------------------------------------
                    string originalDefaultFont = cadImage.DefaultFont;
                    Console.WriteLine($"Original Default Font: {(string.IsNullOrEmpty(originalDefaultFont) ? "(not set)" : originalDefaultFont)}");

                    // -----------------------------------------------------------------
                    // 2. Change the default font for the entire drawing.
                    //    This affects text entities that rely on the document's default font.
                    // -----------------------------------------------------------------
                    string newDefaultFont = "Arial";
                    cadImage.DefaultFont = newDefaultFont;
                    Console.WriteLine($"Default Font has been changed to: {newDefaultFont}");

                    // -----------------------------------------------------------------
                    // 3. Enumerate text entities and display their current font information.
                    // -----------------------------------------------------------------
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity is CadText textEntity)
                        {
                            // Use ObjectHandle from the base class and DefaultValue for the text string.
                            Console.WriteLine($"Text Entity – Handle: {textEntity.ObjectHandle}, Text: \"{textEntity.DefaultValue}\"");
                        }
                    }

                    // -----------------------------------------------------------------
                    // 4. Save the modified drawing back to a CAD format while preserving editability.
                    // -----------------------------------------------------------------
                    DxfOptions dxfSaveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,
                        DxfFileFormat = CadFileFormat.Binary,
                        Version = DxfOutputVersion.R12
                    };

                    cadImage.Save(outputFilePath, dxfSaveOptions);
                    Console.WriteLine($"Modified drawing saved successfully to \"{outputFilePath}\".");
                }
            }
            catch (Exception ex)
            {
                // Write a concise error message to the error stream.
                Console.Error.WriteLine($"An error occurred during processing: {ex.Message}");
            }
        }
    }
}