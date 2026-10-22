using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadBlockInspection
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "input.dwg"; // placeholder path

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"File not found: {inputPath}");
                return;
            }

            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    foreach (CadEntityBase topEntity in cadImage.Entities)
                    {
                        ProcessEntity(topEntity, cadImage, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }

        private static void ProcessEntity(CadEntityBase entity, CadImage cadImage, int depth)
        {
            string indent = new string(' ', depth * 2);
            Console.WriteLine($"{indent}Entity Type: {entity.GetType().Name}, Layer: {entity.LayerName}");

            if (entity is CadInsertObject insert)
            {
                CadBlockEntity blockDef = null;
                foreach (CadBlockEntity blk in cadImage.BlockEntities.ValuesTyped)
                {
                    if (blk.Name == insert.Name)
                    {
                        blockDef = blk;
                        break;
                    }
                }

                if (blockDef != null)
                {
                    Console.WriteLine($"{indent}Entering block \"{blockDef.Name}\" (Depth {depth + 1})");
                    foreach (CadEntityBase innerEntity in blockDef.Entities)
                    {
                        ProcessEntity(innerEntity, cadImage, depth + 1);
                    }
                    Console.WriteLine($"{indent}Exiting block \"{blockDef.Name}\"");
                }
                else
                {
                    Console.WriteLine($"{indent}Block definition \"{insert.Name}\" not found.");
                }
            }
        }
    }
}