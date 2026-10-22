using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.Primitives;
class Program {
 static int Main() {
  string inputPath = "input.dxf";
  string outputPath = "output/edge-hatches.dxf";
  if (!File.Exists(inputPath)) { Console.Error.WriteLine("Input drawing not found: " + inputPath); return 0; }
  Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputPath)));
  try {
   using (CadImage drawing = (CadImage)Image.Load(inputPath)) {
    var model = drawing.Layouts.Values.Single(layout => layout.LayoutName.Equals("Model", StringComparison.OrdinalIgnoreCase));
    for (int region = 0; region < 2; region++) {
     double x = region * 20;
     var corners = new [] { new Point2D(x,0), new Point2D(x+10,0), new Point2D(x+10,10), new Point2D(x,10) };
     var edgePath = new CadEdgeBoundaryPath {
      NumberOfEdges = 4, EdgeTypes = new List<short> { 1,1,1,1 }, Objects = new List<ICadBoundaryPathEntity>()
     };
     for (int i = 0; i < 4; i++) edgePath.Objects.Add(new CadBoundaryPathLine {
      FirstPoint = corners[i], SecondPoint = corners[(i+1)%4]
     });
     var container = new CadHatchBoundaryPathContainer {
      PathType = 0, BoundaryPath = new List<ICadBoundaryPath> { edgePath }, SourceBoundaryObjects = new List<string>()
     };
     var hatch = new CadHatch {
      ObjectHandle = Guid.NewGuid().ToString("N").Substring(0,12).ToUpperInvariant(),
      SoftOwner = model.BlockTableRecordHandle, LayerName = "0", ColorId = (short)(region + 1),
      PatternName = "SOLID", SolidFillFlag = 1, NumberOfBoundaries = 1, NumberOfPatternDefinitions = 0,
      BoundaryPaths = new List<CadHatchBoundaryPathContainer> { container }, PatternDefinitions = new List<CadHatchPatternData>()
     };
     drawing.Entities = drawing.Entities.Concat(new CadEntityBase[] { hatch }).ToArray();
    }
    drawing.UpdateSize(true);
    drawing.Save(outputPath, new DxfOptions { OutputMode = CadOutputMode.Convert });
   }
   using (CadImage reload = (CadImage)Image.Load(outputPath)) {
    int regions = reload.Entities.OfType<CadHatch>().Count(h => h.BoundaryPaths.Any(p => p.BoundaryPath.OfType<CadEdgeBoundaryPath>().Any(e => e.NumberOfEdges == 4)));
    Console.WriteLine("Persisted edge hatch regions: " + regions);
    if (regions < 2) throw new InvalidOperationException("The edge hatch boundaries did not persist.");
   }
   return 0;
  } catch(Exception ex) { Console.Error.WriteLine(ex.Message); return 1; }
 }
}