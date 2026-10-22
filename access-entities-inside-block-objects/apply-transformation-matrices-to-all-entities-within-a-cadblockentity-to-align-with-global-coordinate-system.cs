using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    // This example handles lines, circles, arcs, and nested block inserts with
    // positive uniform XY scales and the standard +Z extrusion direction.
    // Unsupported geometry fails explicitly before any output is written.
    static int Main(string[] args)
    {
        string inputPath = args.Length > 0 ? args[0] : "sample.dxf";
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return 0;
        }
        try
        {
            using Image image = Image.Load(inputPath);
            if (image is not CadImage cad) throw new NotSupportedException("A DWG or DXF drawing is required.");
            var flattened = new List<CadEntityBase>();
            foreach (CadEntityBase entity in cad.Entities)
                TransformEntity(cad, entity, Identity(), new HashSet<string>(), flattened);
            // Every output entity now has world coordinates. Shared definitions
            // stay intact while distinct inserts produce distinct entity copies.
            cad.Entities = flattened.ToArray();
            string outputDir = "output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "global_blocks.pdf");
            var options = new PdfOptions
            {
                VectorRasterizationOptions = new CadRasterizationOptions { PageWidth = 800, PageHeight = 600 }
            };
            cad.Save(outputPath, options);
            Console.WriteLine($"Transformed {flattened.Count} entities into global coordinates: {outputPath}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return 1;
        }
    }

    static void TransformEntity(CadImage cad, CadEntityBase entity, double[,] parent,
                                HashSet<string> active, List<CadEntityBase> output)
    {
        if (entity is CadInsertObject insert)
        {
            RequireStandardNormal(insert.ExtrusionDirection);
            if (insert.ScaleX <= 0 || Math.Abs(insert.ScaleX - insert.ScaleY) > 1e-9 || insert.ScaleZ <= 0)
                throw new NotSupportedException("This example requires positive uniform XY insert scaling.");
            if (insert.RowCount > 1 || insert.ColumnCount > 1)
                throw new NotSupportedException("Array inserts require separate row and column transforms.");
            string name = insert.Name ?? insert.OriginalBlockName;
            if (string.IsNullOrEmpty(name) || !cad.BlockEntities.TryGetValue(name, out CadBlockEntity block))
                throw new InvalidOperationException("Referenced block definition was not found.");
            if (!active.Add(name)) throw new InvalidOperationException($"Cyclic block reference: {name}");
            try
            {
                // World = parent * translate(insert) * rotate * scale * translate(-base).
                double angle = insert.RotationAngle * Math.PI / 180.0;
                double c = Math.Cos(angle), s = Math.Sin(angle);
                double[,] local = Identity();
                local[0, 0] = c * insert.ScaleX; local[0, 1] = -s * insert.ScaleY;
                local[1, 0] = s * insert.ScaleX; local[1, 1] = c * insert.ScaleY;
                local[2, 2] = insert.ScaleZ;
                Cad3DPoint b = block.BasePoint;
                local[0, 3] = insert.InsertionPoint.X - local[0, 0] * b.X - local[0, 1] * b.Y;
                local[1, 3] = insert.InsertionPoint.Y - local[1, 0] * b.X - local[1, 1] * b.Y;
                local[2, 3] = insert.InsertionPoint.Z - insert.ScaleZ * b.Z;
                double[,] world = Multiply(parent, local);
                foreach (CadEntityBase child in block.Entities)
                    TransformEntity(cad, child, world, active, output);
            }
            finally { active.Remove(name); }
            return;
        }

        CadEntityBase transformed;
        double scale = Math.Sqrt(parent[0, 0] * parent[0, 0] + parent[1, 0] * parent[1, 0]);
        double rotation = Math.Atan2(parent[1, 0], parent[0, 0]) * 180.0 / Math.PI;
        switch (entity)
        {
            case CadLine line:
                var first = Apply(parent, line.FirstPoint);
                var second = Apply(parent, line.SecondPoint);
                transformed = new CadLine(first, second);
                Console.WriteLine($"Global line: ({first.X}, {first.Y}, {first.Z}) -> ({second.X}, {second.Y}, {second.Z})");
                break;
            case CadArc arc:
                RequireStandardNormal(arc.ExtrusionDirection);
                transformed = new CadArc(Apply(parent, arc.CenterPoint), arc.Radius * scale,
                                         arc.StartAngle + rotation, arc.EndAngle + rotation)
                { Thickness = arc.Thickness * parent[2, 2] };
                break;
            case CadCircle circle:
                RequireStandardNormal(circle.ExtrusionDirection);
                transformed = new CadCircle(Apply(parent, circle.CenterPoint), circle.Radius * scale)
                { Thickness = circle.Thickness * parent[2, 2] };
                break;
            default:
                throw new NotSupportedException($"No verified geometry transform is implemented for {entity.GetType().Name}.");
        }
        transformed.LayerName = entity.LayerName;
        transformed.ColorId = entity.ColorId;
        transformed.ColorValue = entity.ColorValue;
        output.Add(transformed);
    }

    static void RequireStandardNormal(Cad3DPoint normal)
    {
        if (normal != null && (Math.Abs(normal.X) > 1e-9 || Math.Abs(normal.Y) > 1e-9 || Math.Abs(normal.Z - 1) > 1e-9))
            throw new NotSupportedException("This example requires the standard +Z extrusion direction.");
    }
    static double[,] Identity() => new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
    static double[,] Multiply(double[,] a, double[,] b)
    {
        var result = new double[4, 4];
        for (int row = 0; row < 4; row++)
            for (int column = 0; column < 4; column++)
                for (int k = 0; k < 4; k++) result[row, column] += a[row, k] * b[k, column];
        return result;
    }
    static Cad3DPoint Apply(double[,] m, Cad3DPoint p) => new Cad3DPoint(
        m[0, 0] * p.X + m[0, 1] * p.Y + m[0, 2] * p.Z + m[0, 3],
        m[1, 0] * p.X + m[1, 1] * p.Y + m[1, 2] * p.Z + m[1, 3],
        m[2, 0] * p.X + m[2, 1] * p.Y + m[2, 2] * p.Z + m[2, 3]);
}