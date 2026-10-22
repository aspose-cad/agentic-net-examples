"""Check that every published example matches its validation manifest."""
import argparse
import hashlib
import json
from pathlib import Path


def check_manifest(repo: Path) -> int:
    manifest = json.loads((repo / 'validation-manifest.json').read_text())
    index = json.loads((repo / 'index.json').read_text())
    examples = manifest['examples']
    paths = [entry['path'] for entry in examples]
    if len(paths) != len(set(paths)):
        raise ValueError('Duplicate example paths in validation manifest')
    discovered = {p.relative_to(repo).as_posix() for p in repo.rglob('*.cs')
                  if not any(part.startswith('.') for part in p.relative_to(repo).parts)}
    if set(paths) != discovered or len(paths) != index['total_examples']:
        raise ValueError('Manifest, index and source file coverage disagree')
    if manifest['package_version'] != index['package_version']:
        raise ValueError('Manifest and index SDK versions disagree')
    for entry in examples:
        relative = Path(entry['path'])
        if relative.is_absolute() or '..' in relative.parts:
            raise ValueError('Example path must stay inside repository')
        source = repo / relative
        if source.is_symlink() or not source.resolve().is_relative_to(repo.resolve()):
            raise ValueError('Example must be a regular repository source')
        if hashlib.sha256(source.read_bytes()).hexdigest() != entry['sha256']:
            raise ValueError('Source changed after validation: ' + entry['path'])
        if entry['compile_status'] != 'passed' or entry['runtime_status'] != 'passed':
            raise ValueError('Only compiled and executed examples may be published')
    return len(paths)


if __name__ == '__main__':
    parser = argparse.ArgumentParser(__doc__)
    parser.add_argument('--repo', type=Path, default=Path('.'))
    args = parser.parse_args()
    print(f'Manifest verified: {check_manifest(args.repo)} examples')
