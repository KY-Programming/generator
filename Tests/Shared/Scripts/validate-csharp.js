'use strict';

// Compiles the C# a project generated - the counterpart of validate-files.js for the C# writers.
//
// Generated C# lives inside a .NET project, so it looks as if the build already covers it. It does not:
// the Builder deletes the generated output before it builds (Builder/Services/CleanupService.cs), so the
// files written during that build were not on disk when the compiler ran. Only output marked "keep"
// survives, and even that is compiled in its *previous* content. A generator that starts emitting C#
// that does not compile therefore stays green until somebody builds the project a second time by hand.
//
// This closes that gap by compiling the owning project once more, with the fresh output in place:
//
//     dotnet build <csproj> -t:Compile
//
// Only the Compile target - the generator hooks itself into BeforeBuild/AfterBuild (Main/build.targets),
// and neither runs here, so nothing is regenerated and no prepared database is needed a second time.
// The generated files are compiled together with the project's own sources and package references, which
// is what they were written for: a repository referencing its model and Microsoft.Data.Sqlite compiles
// exactly where it is supposed to.
//
// Because a file that is not part of the compilation would pass that check without ever being read, the
// project's Compile items are asked for as well - a generated file the project does not compile is
// reported as an error, not silently skipped.
//
// The last line of stdout is the machine readable result - everything before it is the human log:
//
//     {"state":"passed","errors":0,"validated":2}
//
// Used from a project's own validate.js:
//
//     require('../../../Tests/Shared/Scripts/validate-csharp').run(path.join(__dirname, 'Output'));
//
// Use check() instead of run() to combine it with another validation in the same script - it returns
// { errors, validated } and leaves the reporting to the caller.

const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');
const shared = require('./type-check');

/** Roslyn, MSBuild and NuGet all report the same way: "error CS1002:", "error MSB4057:", "error NU1101:". */
const DIAGNOSTIC = /(^|\s)error\s+[A-Za-z]+\d+/;

/** The marker KY.Generator appends to every file it writes - see Builder/Services/OutputIdScanner.cs. */
const OUTPUT_ID = /outputid:[0-9a-zA-Z-]+/;

/**
 * The project that compiles the generated files: the closest .csproj at or above them. Generated C# is
 * written either into the project directory itself or into a folder below it, so a few levels are enough.
 */
function findProject(directory) {
    let current = directory;
    for (let level = 0; level < 4; level++) {
        const candidates = fs.existsSync(current)
            ? fs.readdirSync(current).filter(name => name.endsWith('.csproj')).sort()
            : [];
        if (candidates.length === 1) {
            return path.join(current, candidates[0]);
        }
        if (candidates.length > 1) {
            shared.fail(`${current} holds more than one .csproj - pass the one to compile as { project }.`, 2);
        }
        const parent = path.dirname(current);
        if (parent === current) {
            break;
        }
        current = parent;
    }
    return null;
}

/**
 * The generated C# below `directory`. Generated output is recognised by its output id marker, so a hand
 * written file next to it is never counted - the same rule the Builder verifies with. Output written
 * with IgnoreOutputId() carries no marker; a directory holding only such files falls back to all of them.
 */
function collectGenerated(directory) {
    const all = shared.collectFiles(directory, '.cs').sort();
    const generated = all.filter(file => OUTPUT_ID.test(fs.readFileSync(file, 'utf8')));
    if (generated.length > 0) {
        return generated;
    }
    if (all.length > 0) {
        console.log('  no output id markers found - every .cs in the directory is treated as generated');
    }
    return all;
}

function msbuild(args, workingDirectory) {
    const result = spawnSync('dotnet', args, { cwd: workingDirectory, encoding: 'utf8', shell: true });
    return { output: `${result.stdout || ''}${result.stderr || ''}`, status: result.status };
}

/** The full paths of the project's Compile items, lower cased for comparison. */
function readCompileItems(project, workingDirectory) {
    const { output } = msbuild([`msbuild "${project}" -getItem:Compile -nologo`], workingDirectory);
    const start = output.indexOf('{');
    if (start < 0) {
        return null;
    }
    try {
        const items = JSON.parse(output.slice(start)).Items.Compile || [];
        return new Set(items.map(item => path.resolve(item.FullPath).toLowerCase()));
    } catch {
        // An SDK that cannot answer this leaves the inclusion unchecked - the compile itself still runs.
        return null;
    }
}

/**
 * Compiles the project the generated files belong to and returns { errors, validated }.
 * `target` is the directory holding the generated C#; { project } names the .csproj when it cannot be
 * found by walking up from there.
 */
function check(target, { project } = {}) {
    if (!target) {
        shared.fail('no directory passed.', 2);
    }

    const directory = path.resolve(target);
    if (!fs.existsSync(directory)) {
        shared.fail(`${directory} does not exist.`, 2);
    }

    const files = collectGenerated(directory);
    if (files.length === 0) {
        // Nothing to compile is a failure, not a pass - a project that quietly stopped generating would
        // otherwise stay green forever.
        shared.fail(`no generated .cs files found in ${directory}.`);
    }

    const csproj = project ? path.resolve(project) : findProject(directory);
    if (csproj === null) {
        shared.fail(`no .csproj found at or above ${directory}.`, 2);
    }
    if (!fs.existsSync(csproj)) {
        shared.fail(`${csproj} does not exist.`, 2);
    }

    const workingDirectory = path.dirname(csproj);
    console.log(`${path.basename(directory)}: ${files.length} generated file(s)`);
    console.log(`  compiled by ${path.basename(csproj)}`);

    let errors = 0;

    // Only the Compile target: the generator runs on BeforeBuild/AfterBuild and must not run again.
    const { output, status } = msbuild([`build "${csproj}" -t:Compile -nologo -v:m`], workingDirectory);
    if (output.trim().length > 0) {
        console.log(output.trimEnd());
    }
    errors += output.split(/\r?\n/).filter(line => DIAGNOSTIC.test(line)).length;
    if (status !== 0 && errors === 0) {
        // The build can fail without a single diagnostic line (a crashed SDK) - still a failure.
        errors = 1;
    }

    // A file the project does not compile would pass the step above without ever being read.
    const compiled = readCompileItems(csproj, workingDirectory);
    if (compiled === null) {
        console.log('  could not read the Compile items - the inclusion of the generated files is unchecked');
    } else {
        for (const file of files) {
            if (!compiled.has(file.toLowerCase())) {
                console.log(`  ${shared.display(file, workingDirectory)} is not compiled by ${path.basename(csproj)}`);
                errors++;
            }
        }
    }

    return { errors, validated: files.length };
}

function run(target, options) {
    const result = check(target, options);
    shared.report(result.errors, result.validated);
}

module.exports = { check, run };

// Also runnable directly: node validate-csharp.js <directory> [<csproj>]
if (require.main === module) {
    run(process.argv[2], { project: process.argv[3] });
}
