'use strict';

// Type-checks the TypeScript that one test project generated.
//
// Which tsconfig a file is checked against is derived from the C# sources, not from a hand written list:
//
//   strict      the default - Tests/Shared/tsconfig.strict.json
//   non strict  the project opted out via [assembly:GenerateNonStrict], or the class itself via
//               [GenerateNonStrict] - Tests/Shared/tsconfig.non-strict.json
//
// Both lists are checked in the same run, so a project that mixes the two is covered by one script.
//
// Files are dropped when they cannot be type-checked standalone:
//
//   - imports a package that is not installed - the installed ones are the devDependencies of
//     Tests/Shared/package.json, so adding a package there is enough to get its output validated
//   - index.ts barrels - they only re-export files that are in the list already
//
// A drop propagates to every file that imports the dropped one, otherwise tsc would pull it back into
// the program through that import. Everything dropped is reported on stdout - nothing is skipped silently.
//
// The last line of stdout is the machine readable result - everything before it is the human log:
//
//     {"state":"passed","errors":0,"validated":35}
//
// The exit code is only a convenience for running this by hand (0 = passed); the Builder reads the JSON.
//
// Used from a project's own validate.js:
//
//     require('../../Shared/Scripts/validate-generated').run(__dirname);

const fs = require('fs');
const path = require('path');
const shared = require('./type-check');

// True when the C# declaration of `typeName` carries [GenerateNonStrict] on one of the attribute lines above it.
function isTypeNonStrict(typeName, sources) {
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
            if (line.includes('GenerateNonStrict')) {
                return true;
            }
        }
    }
    return false;
}

/**
 * Splits the project's generated TypeScript into the strict and the non strict list and returns
 * { strict: string[], nonStrict: string[], dropped: Map<file, reason>, total: number }.
 */
function buildFileLists(projectDirectory) {
    const installed = shared.readInstalledPackages();
    // Projects share their test types - a model may be declared next to the project or in Shared.
    const sharedSourceDirectory = path.join(path.dirname(projectDirectory), 'Shared');
    const ownSources = shared.collectFiles(projectDirectory, '.cs').map(file => fs.readFileSync(file, 'utf8'));
    const sharedSources = shared.collectFiles(sharedSourceDirectory, '.cs').map(file => fs.readFileSync(file, 'utf8'));
    const projectIsNonStrict = ownSources.some(source => source.includes('[assembly:GenerateNonStrict]')
        || source.includes('[assembly: GenerateNonStrict]'));
    const sources = ownSources.concat(sharedSources);

    const files = new Map();
    for (const file of shared.collectFiles(projectDirectory, '.ts')) {
        const content = fs.readFileSync(file, 'utf8');
        const exported = shared.readExportedTypes(content);
        files.set(file, {
            imports: shared.readImports(content),
            // Strict is the default - a file only leaves the strict list when it, or its project, opted out.
            strict: !projectIsNonStrict && !exported.some(name => isTypeNonStrict(name, sources)),
        });
    }

    const dropped = new Map();
    const drop = (file, reason) => {
        if (!dropped.has(file)) {
            dropped.set(file, reason);
        }
    };

    for (const [file, info] of files) {
        if (path.basename(file) === 'index.ts') {
            drop(file, 'index.ts barrel - re-exports files that are in the list already');
        }
        const missing = info.imports
            .filter(specifier => !specifier.startsWith('.'))
            .filter(specifier => !installed.has(shared.toPackageName(specifier)));
        if (missing.length > 0) {
            drop(file, `imports ${[...new Set(missing)].join(', ')} - not a devDependency of Tests/Shared/package.json`);
        }
    }

    // A file that imports a dropped file drags it back into the program, so it has to go as well.
    for (let changed = true; changed;) {
        changed = false;
        for (const [file, info] of files) {
            if (dropped.has(file)) {
                continue;
            }
            for (const specifier of info.imports) {
                if (!specifier.startsWith('.')) {
                    continue;
                }
                const target = path.resolve(path.dirname(file), specifier) + '.ts';
                if (dropped.has(target)) {
                    drop(file, `imports ${shared.display(target, projectDirectory)}, which is not in the list`);
                    changed = true;
                    break;
                }
            }
        }
    }

    const kept = [...files.keys()].filter(file => !dropped.has(file)).sort();
    return {
        strict: kept.filter(file => files.get(file).strict),
        nonStrict: kept.filter(file => !files.get(file).strict),
        dropped,
        total: files.size,
    };
}

function run(target) {
    if (!target) {
        shared.fail('no project directory passed.', 2);
    }

    const projectDirectory = path.resolve(target);
    const lists = buildFileLists(projectDirectory);
    const validated = lists.strict.length + lists.nonStrict.length;

    console.log(`${path.basename(projectDirectory)}: ${validated} of ${lists.total} generated file(s)`);
    for (const [file, reason] of [...lists.dropped].sort()) {
        console.log(`  skipped ${shared.display(file, projectDirectory)} - ${reason}`);
    }

    if (validated === 0) {
        // Nothing to type-check is a failure, not a pass - a project that quietly stopped generating
        // would otherwise stay green forever, which is the exact blind spot this script closes.
        shared.fail(lists.total === 0
                        ? 'the project generated no TypeScript at all.'
                        : 'every generated file was dropped - see the reasons above.');
    }

    shared.report((shared.typeCheck(lists.strict, true, projectDirectory) || 0)
                  + (shared.typeCheck(lists.nonStrict, false, projectDirectory) || 0),
                  validated);
}

module.exports = { run };

// Also runnable directly: node validate-generated.js <projectDirectory>
if (require.main === module) {
    run(process.argv[2]);
}
