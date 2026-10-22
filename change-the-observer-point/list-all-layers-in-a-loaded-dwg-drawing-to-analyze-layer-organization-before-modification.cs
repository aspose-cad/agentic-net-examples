using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace ListCadLayers
{
    class Program
    {
        static void Main(string[] args)
        {
            // Entry point wrapped in a try/catch to report any unexpected errors without crashing.
            try
            {
                // Determine the path to the DWG file. If a command‑line argument is supplied, use it;
                // otherwise fall back to a default file name in the current directory.
                string sourcePath = args.Length > 0 ? args[0] : "sample.dwg";

                // Verify that the source file exists before attempting to load it.
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD drawing using the generic Aspose.CAD.Image loader.
                // The returned Image is cast to CadImage to gain access to CAD‑specific tables.
                using (Image genericImage = Image.Load(sourcePath))
                {
                    // Ensure the loaded image is indeed a CAD image.
                    if (genericImage is not CadImage cadImage)
                    {
                        Console.Error.WriteLine("The loaded file is not a supported CAD format.");
                        return;
                    }

                    // The Layers collection contains CadLayerTable objects representing each layer.
                    // Iterate through the collection and output relevant information.
                    Console.WriteLine($"Layers in '{Path.GetFileName(sourcePath)}':");
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        // Basic layer attributes.
                        string layerName = layer.Name;
                        int colorId = layer.ColorId;
                        int flags = layer.Flags;

                        // Determine visibility status based on CAD conventions.
                        bool isOff = colorId < 0;                     // Negative ColorId indicates the layer is turned off.
                        bool isFrozen = (flags & 1) != 0;             // Bit 0 set means the layer is frozen.
                        bool isLocked = (flags & 4) != 0;             // Bit 2 (value 4) set means the layer is locked.

                        // Build a readable status string.
                        string status = "";
                        if (isOff) status += "Off ";
                        if (isFrozen) status += "Frozen ";
                        if (isLocked) status += "Locked ";
                        if (status.Length == 0) status = "Visible";

                        // Output the layer details.
                        Console.WriteLine($"- Name: {layerName}, ColorId: {colorId}, Status: {status.Trim()}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Write a concise error message to the error stream.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}