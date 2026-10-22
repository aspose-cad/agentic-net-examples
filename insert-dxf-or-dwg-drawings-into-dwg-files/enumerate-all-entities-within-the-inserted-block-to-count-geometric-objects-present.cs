using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadBlockEntityEnumeration
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string sourcePath = "input.dwg";

                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                using (Image image = Image.Load(sourcePath))
                {
                    CadImage cadImage = (CadImage)image;

                    int totalGeometricObjects = 0;

                    foreach (CadEntityBase topEntity in cadImage.Entities)
                    {
                        if (topEntity is CadInsertObject insertObject)
                        {
                            string blockName = insertObject.Name;

                            if (cadImage.BlockEntities != null && cadImage.BlockEntities.ContainsKey(blockName))
                            {
                                CadBlockEntity blockEntity = cadImage.BlockEntities[blockName];

                                int blockGeometricCount = 0;

                                foreach (CadEntityBase blockEntityItem in blockEntity.Entities)
                                {
                                    if (blockEntityItem is CadLine ||
                                        blockEntityItem is CadLwPolyline ||
                                        blockEntityItem is Cad3DFace)
                                    {
                                        blockGeometricCount++;
                                    }
                                }

                                Console.WriteLine($"Block \"{blockName}\" contains {blockGeometricCount} geometric objects.");
                                totalGeometricObjects += blockGeometricCount;
                            }
                            else
                            {
                                Console.WriteLine($"Block \"{blockName}\" not found in the drawing.");
                            }
                        }
                    }

                    Console.WriteLine($"Total geometric objects in all inserted blocks: {totalGeometricObjects}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}