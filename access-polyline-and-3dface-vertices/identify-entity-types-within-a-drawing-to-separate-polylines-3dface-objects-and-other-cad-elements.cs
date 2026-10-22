using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        // Path to the CAD file
        string sourceFile = "sample.dwg";

        if (!File.Exists(sourceFile))
        {
            Console.Error.WriteLine($"File not found: {sourceFile}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(sourceFile))
            {
                // Cast to CadImage to access the CAD object model
                CadImage cadImage = (CadImage)image;

                // Collections to hold separated entities
                List<CadLwPolyline> polylines = new List<CadLwPolyline>();
                List<Cad3DFace> faces3D = new List<Cad3DFace>();
                List<CadEntityBase> otherEntities = new List<CadEntityBase>();

                // Iterate over top-level entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    switch (entity)
                    {
                        case CadLwPolyline polyline:
                            polylines.Add(polyline);
                            break;
                        case Cad3DFace face:
                            faces3D.Add(face);
                            break;
                        default:
                            otherEntities.Add(entity);
                            break;
                    }
                }

                // Output counts
                Console.WriteLine($"Polylines count: {polylines.Count}");
                Console.WriteLine($"3D Face count: {faces3D.Count}");
                Console.WriteLine($"Other entities count: {otherEntities.Count}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}