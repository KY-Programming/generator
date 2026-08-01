'use strict';

// Builds the tsc file list for the TypeScriptValidationStrict / TypeScriptValidationNotStrict projects.
//
// Both validation projects type-check the TypeScript that the other v10 projects generate. Which files
// belong to which project is derived from the C# sources here, not from a hand written list:
//
//   strict  = the project has [assembly:GenerateStrict], or the class itself has [GenerateStrict]
//   all     = every generated file (strict output is valid under a non strict tsconfig too)
//
// Files are dropped when they cannot be type-checked standalone:
//
//   - imports a package that is not installed - the installed ones are the devDependencies of
//     Tests/v10/package.json, so adding a package there is enough to get its output validated
//   - index.ts barrels - they only re-export files that are in the list already
//
// A drop propagates to every file that imports the dropped one, otherwise tsc would pull it back into
// the program through that import. Everything dropped is reported on stdout - nothing is skipped silently.
//
// Used by validate-typescript.js, which runs tsc over the result and reports the outcome as JSON.
//
// Usage: node build-file-list.js --strict|--all
// Writes tsconfig.generated.json into the current directory.

const fs = require('fs');
const path = require('path');

const V10_ROOT = path.resolve(__dirname, '..', '..');
const SHARED_DIRECTORY = path.join(V10_ROOT, 'Shared');
const VALIDATION_PROJECTS = ['TypeScriptValidationStrict', 'TypeScriptValidationNotStrict'];
const IGNORED_DIRECTORIES = new Set(['bin', 'obj', 'node_modules', '.idea', '.vs', '.git', 'Properties']);
const OUTPUT_FILE = 'tsconfig.generated.json';

function collectFiles(directory, extension, found = []) {
    if (!fs.existsSync(directory)) {
        return found;
    }
    for (const entry of fs.readdirSync(directory, { withFileTypes: true })) {
        const full = path.join(directory, entry.name);
        if (entry.isDirectory()) {
            if (!IGNORED_DIRECTORIES.has(entry.name)) {
                collectFiles(full, extension, found);
            }
        } else if (entry.name.endsWith(extension)) {
            found.push(full);
        }
    }
    return found;
}

// '@angular/common/http' -> '@angular/common', 'rxjs/operators' -> 'rxjs'
function toPackageName(specifier) {
    const parts = specifier.split('/');
    return specifier.startsWith('@') ? parts.slice(0, 2).join('/') : parts[0];
}

function readInstalledPackages() {
    const manifest = JSON.parse(fs.readFileSync(path.join(V10_ROOT, 'package.json'), 'utf8'));
    return new Set(Object.keys(manifest.devDependencies || {}));
}

function toKey(file) {
    return path.relative(V10_ROOT, file).split(path.sep).join('/');
}

function listProjects() {
    return fs.readdirSync(V10_ROOT, { withFileTypes: true })
        .filter(entry => entry.isDirectory())
        .map(entry => entry.name)
        .filter(name => name !== 'Shared' && !VALIDATION_PROJECTS.includes(name) && !IGNORED_DIRECTORIES.has(name));
}

function readImports(content) {
    const imports = [];
    const pattern = /(?:^|\n)\s*(?:import|export)\b[^;'"]*?from\s*['"]([^'"]+)['"]/g;
    let match;
    while ((match = pattern.exec(content)) !== null) {
        imports.push(match[1]);
    }
    return imports;
}

function readExportedTypes(content) {
    const names = [];
    const pattern = /^export\s+(?:declare\s+)?(?:abstract\s+)?(?:class|interface|enum|type|const|function)\s+([A-Za-z_$][\w$]*)/gm;
    let match;
    while ((match = pattern.exec(content)) !== null) {
        names.push(match[1]);
    }
    return names;
}

// True when the C# declaration of `typeName` carries [GenerateStrict] on one of the attribute lines above it.
function isTypeStrict(typeName, sources) {
    const declaration = new RegExp(`(?:^|\\s)(?:class|interface|enum|struct|record(?:\\s+struct|\\s+class)?)\\s+${typeName}\\b`);
    for (const source of sources) {
        const match = declaration.exec(source);
        if (match === null) {
            continue;
        }
        // match.index sits on the whitespace before the keyword, so cut at the start of the declaring line.
        const lines = source.slice(0, source.lastIndexOf('\n', match.index) + 1).split('\n');
        for (let index = lines.length - 1; index >= 0; index--) {
            const line = lines[index].trim();
            if (line === '' || line.startsWith('//')) {
                continue;
            }
            if (!line.startsWith('[')) {
                break;
            }
            if (line.includes('GenerateStrict')) {
                return true;
            }
        }
    }
    return false;
}

/**
 * Writes tsconfig.generated.json into projectDirectory and returns
 * { included: string[], dropped: Map<file, reason>, total: number }.
 */
function buildFileList(mode, projectDirectory) {
    const installed = readInstalledPackages();
    const sharedSources = collectFiles(SHARED_DIRECTORY, '.cs').map(file => fs.readFileSync(file, 'utf8'));

    const files = new Map();
    for (const project of listProjects()) {
        const projectDirectory = path.join(V10_ROOT, project);
        const ownSources = collectFiles(projectDirectory, '.cs').map(file => fs.readFileSync(file, 'utf8'));
        const projectIsStrict = ownSources.some(source => source.includes('[assembly:GenerateStrict]')
            || source.includes('[assembly: GenerateStrict]'));
        const sources = ownSources.concat(sharedSources);

        for (const file of collectFiles(projectDirectory, '.ts')) {
            const content = fs.readFileSync(file, 'utf8');
            const exported = readExportedTypes(content);
            files.set(toKey(file), {
                path: file,
                project,
                imports: readImports(content),
                strict: projectIsStrict || (exported.length > 0 && exported.every(name => isTypeStrict(name, sources))),
            });
        }
    }

    const dropped = new Map();
    const drop = (key, reason) => {
        if (!dropped.has(key)) {
            dropped.set(key, reason);
        }
    };

    for (const [key, file] of files) {
        if (path.basename(file.path) === 'index.ts') {
            drop(key, 'index.ts barrel - re-exports files that are in the list already');
        }
        const missing = file.imports
            .filter(specifier => !specifier.startsWith('.'))
            .filter(specifier => !installed.has(toPackageName(specifier)));
        if (missing.length > 0) {
            drop(key, `imports ${[...new Set(missing)].join(', ')} - not a devDependency of Tests/v10/package.json`);
        }
        if (mode === 'strict' && !file.strict) {
            drop(key, 'generated without GenerateStrict - covered by TypeScriptValidationNotStrict');
        }
    }

    // A file that imports a dropped file drags it back into the program, so it has to go as well.
    for (let changed = true; changed;) {
        changed = false;
        for (const [key, file] of files) {
            if (dropped.has(key)) {
                continue;
            }
            for (const specifier of file.imports) {
                const target = toKey(path.resolve(path.dirname(file.path), specifier) + '.ts');
                if (dropped.has(target)) {
                    drop(key, `imports ${target}, which is not in the list`);
                    changed = true;
                    break;
                }
            }
        }
    }

    const included = [...files.keys()]
        .filter(key => !dropped.has(key))
        .sort()
        .map(key => path.relative(projectDirectory, files.get(key).path).split(path.sep).join('/'));

    const header = '// Generated by Shared/Scripts/build-file-list.js - do not edit, do not commit.\n';
    fs.writeFileSync(
        path.join(projectDirectory, OUTPUT_FILE),
        header + JSON.stringify({ extends: './tsconfig.json', files: included }, null, 2) + '\n',
        'utf8');

    return { included, dropped, total: files.size };
}

/** Prints what the list covers and what it leaves out - nothing is skipped silently. */
function report(result, mode) {
    console.log(`${OUTPUT_FILE}: ${result.included.length} of ${result.total} generated files (mode: ${mode})`);
    for (const [key, reason] of [...result.dropped].sort()) {
        console.log(`  skipped ${key} - ${reason}`);
    }
}

function parseMode(argv) {
    return argv.includes('--strict') ? 'strict' : argv.includes('--all') ? 'all' : null;
}

module.exports = { buildFileList, report, parseMode, OUTPUT_FILE, V10_ROOT };

if (require.main === module) {
    const mode = parseMode(process.argv);
    if (mode === null) {
        console.error('Usage: node build-file-list.js --strict|--all');
        process.exit(1);
    }
    report(buildFileList(mode, process.cwd()), mode);
}
