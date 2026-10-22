# Validation and publication coverage

This batch contains **1,864 standalone C# examples** from a 1,959-task catalog.
**95 cached sources were withheld** because they fail current SDK or task guards.
A cached `present` state is not publication evidence. The validation manifest
records every included task, source path, SHA-256, and compile/run result.

All included sources were freshly compiled and executed with .NET 8 and
Aspose.CAD 26.9.0. Most checks exercise a safe missing-input exit; supply an
appropriate CAD drawing to verify the requested operation and exported geometry.
These checks do not establish drawing-specific behavior for all 1,864 examples.
A missing-input failure in task 44008 was corrected at the entrypoint; actual
conversion errors still return failure. That source also exported a real DXF
to PDF successfully. Original audit failures remain recorded in the generator.

The GitHub workflow runs on pull requests, main pushes, manual dispatches and
weekly schedules. Eight shards compile and run the full published corpus. The
aggregate gate requires 100% success and complete shard coverage. Manifest
checks reject changed sources and missing or additional unverified files.
JSON, JUnit, Markdown and HTML results are retained as workflow artifacts.

```bash
python .github/scripts/check_manifest.py
python .github/scripts/validate_example_corpus.py --repo . --shards 1 --shard 0 \
  --cad-version 26.9.0 --out validation/shard-0.json --junit validation/junit-0.xml
```

The only direct validation dependency is the pinned Aspose.CAD NuGet package;
its framework dependencies are restored by NuGet. No SDK DLLs, credentials,
license files, generated build workspaces or customer drawings are committed.
