using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                // Path to the CAD file
                string inputPath = "input.dwg";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Collections for polylines and 3DFace entities
                    List<CadLwPolyline> polylineCollection = new List<CadLwPolyline>();
                    List<Cad3DFace> faceCollection = new List<Cad3DFace>();

                    // Iterate over top-level entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Identify lightweight polylines
                        if (entity is CadLwPolyline polyline)
                        {
                            polylineCollection.Add(polyline);
                        }
                        // Identify 3D face entities
                        else if (entity is Cad3DFace face)
                        {
                            faceCollection.Add(face);
                        }
                    }

                    // Output counts
                    Console.WriteLine($"Polylines found: {polylineCollection.Count}");
                    Console.WriteLine($"3DFace entities found: {faceCollection.Count}");
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}