using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.UnderlayDefinition;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                // Path to the CAD drawing to be validated
                string cadFilePath = "sample.dwg";

                // Load the CAD drawing
                using (Image image = Image.Load(cadFilePath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Iterate through all CAD objects to find external references
                    foreach (CadObjectBase cadObject in cadImage.Objects)
                    {
                        // Check for external raster image definitions
                        if (cadObject is CadRasterImageDef rasterDef)
                        {
                            string rasterPath = rasterDef.FileName;
                            bool rasterExists = File.Exists(rasterPath);
                            Console.WriteLine($"Raster Image Definition (Handle: {rasterDef.ObjectHandle}) - File: \"{rasterPath}\" Exists: {rasterExists}");
                        }
                        // Check for underlay definitions
                        else if (cadObject is CadUnderlayDefinition underlayDef)
                        {
                            string underlayPath = underlayDef.UnderlayPathAndFileName;
                            bool underlayExists = File.Exists(underlayPath);
                            Console.WriteLine($"Underlay Definition (Handle: {underlayDef.ObjectHandle}) - File: \"{underlayPath}\" Exists: {underlayExists}");
                        }
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