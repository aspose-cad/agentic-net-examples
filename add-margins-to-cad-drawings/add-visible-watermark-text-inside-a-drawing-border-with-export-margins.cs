using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;

// Task 45026: owner clarified the requested output as VISIBLE watermark text.
// Usage: example [input.dwg|input.dxf] [new-output-directory] ["REVIEW COPY"]
// The border surrounds the original extents and a reserved caption strip.
// Export margins add page space; existing drawing entities are never moved.
class Program
{
    static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    static string Handle() { return "F" + Guid.NewGuid().ToString("N").Substring(0, 14).ToUpperInvariant(); }

    static void AddToModel(CadImage drawing, CadLayout model, CadEntityBase entity)
    {
        var block = drawing.BlockEntities.Values.Cast<CadBlockEntity>()
            .Single(b => b.CadBlockTableObject?.ObjectHandle == model.BlockTableRecordHandle);
        entity.SoftOwner = model.BlockTableRecordHandle;
        entity.ObjectHandle = Handle();
        block.AddEntity(entity);
        drawing.Entities = drawing.Entities.Concat(new[] { entity }).ToArray();
    }

    static int Main(string[] args)
    {
        try
        {
            Require(args.Length <= 3, "Usage: example [input.dwg|input.dxf] [new-output-directory] [watermark text]");
            string input = args.Length > 0 ? args[0] : "input.dwg";
            if (!File.Exists(input))
            {
                Console.Error.WriteLine("Input file not found: " + input);
                return args.Length == 0 ? 0 : 1;
            }
            string text = args.Length > 2 ? args[2] : "REVIEW COPY";
            Require(!string.IsNullOrWhiteSpace(text) && text.Length <= 40 && text.All(c => c >= 32 && c <= 126),
                "Use 1–40 printable ASCII characters for this bounded watermark example.");
            string directory = args.Length > 1 ? args[1] : "output";
            Directory.CreateDirectory(directory);
            string native = Path.Combine(directory, "visible-watermark.dxf");
            string pdf = Path.Combine(directory, "visible-watermark.pdf");
            string pending = Path.Combine(directory, "visible-watermark.pending.dxf");
            string pendingPdf = Path.Combine(directory, "visible-watermark.pending.pdf");
            Require(new[] { native, pdf, pending, pendingPdf }.All(p => !File.Exists(p)),
                "Output already exists; choose a new output directory.");
            string[] originalHandles, originalLayouts;
            string textHandle, borderHandle, owner;
            double left, right, bottom, top, x, y, height;
            int originalCount;
            using (Image loaded = Image.Load(input))
            {
                CadImage drawing = loaded as CadImage;
                Require(drawing != null, "A native DWG or DXF drawing is required.");
                CadLayout model = drawing.Layouts.Values.SingleOrDefault(l =>
                    l.LayoutName.Equals("Model", StringComparison.OrdinalIgnoreCase));
                Require(model != null, "Model layout is missing.");
                owner = model.BlockTableRecordHandle;
                originalHandles = drawing.Entities.Select(e => e.ObjectHandle).ToArray();
                originalCount = originalHandles.Length;
                originalLayouts = drawing.Layouts.Values.Select(l => l.LayoutName).OrderBy(n => n).ToArray();
                Require(originalCount > 0 && originalHandles.All(h => !string.IsNullOrWhiteSpace(h)),
                    "This example requires existing entities with persistent handles.");
                drawing.UpdateSize(true);
                double minX = drawing.MinPoint.X, minY = drawing.MinPoint.Y;
                double maxX = drawing.MaxPoint.X, maxY = drawing.MaxPoint.Y;
                double span = Math.Max(maxX - minX, maxY - minY);
                Require(double.IsFinite(span) && span > 0 && new[] { minX, minY, maxX, maxY }.All(double.IsFinite),
                    "Drawing extents must be finite and non-empty.");
                double padding = span * 0.05;
                left = minX - padding; right = maxX + padding;
                top = maxY + padding; bottom = minY - 4 * padding;
                height = Math.Min(padding * 0.75, (right - left - 2 * padding) / (1.5 * text.Length));
                x = left + padding; y = bottom + padding;
                Require(height > 0 && x + 1.5 * height * text.Length < right && y + 2 * height < minY,
                    "The caption cannot fit inside the reserved border strip.");
                string layer = "VISIBLE_WATERMARK_" + Guid.NewGuid().ToString("N").Substring(0, 8);
                drawing.Layers.Add(new CadLayerTable { Name = layer, ColorId = 7, Flags = 0, LineTypeName = "CONTINUOUS" });
                var border = new CadLwPolyline
                {
                    Flag = CadLwPolylineFlag.Closed, PointCount = 4, LayerName = layer, ColorId = 5,
                    Coordinates = new List<Cad2DPoint> { new Cad2DPoint(left, bottom), new Cad2DPoint(right, bottom),
                        new Cad2DPoint(right, top), new Cad2DPoint(left, top) }
                };
                var caption = new CadText { DefaultValue = text, FirstAlignment = new Cad3DPoint(x, y, 0),
                    TextHeight = height, ScaleX = 1, TextRotation = 0, LayerName = layer, ColorId = 1 };
                AddToModel(drawing, model, border); AddToModel(drawing, model, caption);
                textHandle = caption.ObjectHandle; borderHandle = border.ObjectHandle;
                drawing.Save(pending, new DxfOptions { OutputMode = CadOutputMode.Convert });
            }
            using (var saved = (CadImage)Image.Load(pending))
            {
                var entities = saved.Entities.ToArray();
                Require(entities.Length == originalCount + 2 && originalHandles.All(h => entities.Any(e => e.ObjectHandle == h)),
                    "Existing entities were lost or unexpected entities appeared.");
                Require(originalLayouts.SequenceEqual(saved.Layouts.Values.Select(l => l.LayoutName).OrderBy(n => n)),
                    "Existing layouts changed.");
                var caption = entities.OfType<CadText>().Single(t => t.ObjectHandle == textHandle);
                var border = entities.OfType<CadLwPolyline>().Single(p => p.ObjectHandle == borderHandle);
                Require(caption.DefaultValue == text && caption.SoftOwner == owner && border.SoftOwner == owner &&
                    caption.FirstAlignment.X == x && caption.FirstAlignment.Y == y && caption.TextHeight == height &&
                    caption.ColorId == 1 && border.Coordinates.Count == 4 && border.Flag == CadLwPolylineFlag.Closed &&
                    border.Coordinates[0].X == left && border.Coordinates[0].Y == bottom &&
                    border.Coordinates[2].X == right && border.Coordinates[2].Y == top,
                    "Visible caption, border geometry or model ownership did not persist.");
                bool complete = false; int failures = 0, callbacks = 0;
                var raster = new CadRasterizationOptions { PageWidth = 1000, PageHeight = 800, UnitType = UnitType.Point,
                    Layouts = new[] { "Model" }, DrawType = CadDrawTypeMode.UseObjectColor, Margins = new Margins { Left = 25, Right = 25, Top = 25, Bottom = 25 } };
                raster.RenderResult = result => { callbacks++; complete = result.IsRenderComplete; failures += result.Failures.Count; };
                saved.Save(pendingPdf, new PdfOptions { VectorRasterizationOptions = raster });
                Require(callbacks > 0 && complete && failures == 0 && File.Exists(pendingPdf) && new FileInfo(pendingPdf).Length > 100,
                    "The visible-watermark PDF did not render completely.");
            }
            File.Move(pending, native); File.Move(pendingPdf, pdf);
            Console.WriteLine(JsonSerializer.Serialize(new { text, textHandle, borderHandle, owner, originalCount,
                left, right, bottom, top, x, y, height, pageMargins = 25, native, pdf }));
            Console.WriteLine("Verified task 45026: visible text and border persisted; PDF render completed.");
            return 0;
        }
        catch (Exception ex) { Console.Error.WriteLine(ex.Message); return 1; }
    }
}
