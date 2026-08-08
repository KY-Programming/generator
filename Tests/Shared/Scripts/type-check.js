'use strict';

// The pieces every validation script shares: finding files, reading their imports, and running tsc over
// a list of them against one of the shared tsconfigs.
//
// The generated config is written to the temp folder, never next to the files being checked, so nothing
// lands beside verified output.
//
// Not a script - see validate-generated.js, validate-files.js and validate-no-output.js, which use this.

const fs = require('fs');
const os = require('os');
const path = require('path');
const { spawnSync } = require('child_process');
const { ensurePackages } = require('./ensure-packages');

/** Tests/Shared - holds the scripts and the two tsconfigs. */
const SHARED_ROOT = path.resolve(__dirname, '..');

/**
 * Tests - holds package.json and the single node_modules of the validation.
 *
 * It sits one level above Tests/Shared on purpose: TypeScript resolves a package by walking up from the
 * file that imports it, so node_modules has to be an ancestor of the checked output. From Tests/Shared
 * it would be a sibling of Tests/v10 and nothing would resolve.
 */
const PACKAGE_ROOT = path.resolve(SHARED_ROOT, '..');
const TSC = path.join(PACKAGE_ROOT, 'node_modules', 'typescript', 'bin', 'tsc');
const IGNORED_DIRECTORIES = new Set(['bin', 'obj', 'node_modules', '.idea', '.vs', '.git', 'Properties']);
const DIAGNOSTIC = /error TS\d+/;

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

/** '@angular/common/http' -> '@angular/common', 'rxjs/operators' -> 'rxjs' */
function toPackageName(specifier) {
    const parts = specifier.split('/');
    return specifier.startsWith('@') ? parts.slice(0, 2).join('/') : parts[0];
}

/** The packages generated output may import - adding one here is enough to get its output validated. */
function readInstalledPackages() {
    const manifest = JSON.parse(fs.readFileSync(path.join(PACKAGE_ROOT, 'package.json'), 'utf8'));
    return new Set(Object.keys(manifest.devDependencies || {}));
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

/**
 * Runs tsc over one list of files against the shared strict or non strict tsconfig.
 * Returns the number of diagnostics (0 = clean), or null when the list is empty.
 */
function typeCheck(files, strict, workingDirectory) {
    if (files.length === 0) {
        return null;
    }
    const failure = ensurePackages(PACKAGE_ROOT);
    if (failure !== null) {
        fail(failure);
    }

    const configName = strict ? 'tsconfig.strict.json' : 'tsconfig.non-strict.json';
    const base = path.join(SHARED_ROOT, configName);
    const configPath = path.join(os.tmpdir(), `ky-generator-validate-${process.pid}-${configName}`);
    // Only "files" is set here - the packages are found by walking up from the checked files to
    // Tests/node_modules. A "paths" mapping would look like an alternative, but it bypasses the
    // "exports" map of a package, and that is the only way "@angular/common/http" resolves.
    fs.writeFileSync(configPath, JSON.stringify({ extends: base, files }, null, 2) + '\n', 'utf8');
    console.log(`  ${files.length} file(s) against ${configName}`);

    try {
        const tsc = spawnSync(process.execPath, [TSC, '-p', configPath], { cwd: workingDirectory, encoding: 'utf8' });
        const output = `${tsc.stdout || ''}${tsc.stderr || ''}`;
        if (output.trim().length > 0) {
            console.log(output.trimEnd());
        }
        const errors = output.split(/\r?\n/).filter(line => DIAGNOSTIC.test(line)).length;
        // tsc can fail without a single diagnostic line (bad config, crash) - that is still a failure.
        return tsc.status === 0 && errors === 0 ? 0 : Math.max(errors, 1);
    } finally {
        try { fs.unlinkSync(configPath); } catch { /* the temp file is disposable */ }
    }
}

/** Prints the machine readable result the Builder reads and ends the process. */
function report(errors, validated) {
    console.log(errors === 0 ? 'VALIDATION PASSED' : `VALIDATION FAILED: ${errors} error(s).`);
    console.log(JSON.stringify({ state: errors === 0 ? 'passed' : 'failed', errors, validated }));
    process.exit(errors === 0 ? 0 : 1);
}

/** Prints a failure that is not a type error - a bad argument, a missing folder, an empty project. */
function fail(message, exitCode = 1) {
    console.log(`VALIDATION FAILED: ${message}`);
    console.log(JSON.stringify({ state: 'failed', errors: 1, validated: 0 }));
    process.exit(exitCode);
}

/** Repository relative-ish path for the log - keeps the output readable. */
function display(file, from) {
    return path.relative(from, file).split(path.sep).join('/');
}

module.exports = {
    PACKAGE_ROOT,
    SHARED_ROOT,
    collectFiles,
    display,
    fail,
    readExportedTypes,
    readImports,
    readInstalledPackages,
    report,
    toPackageName,
    typeCheck,
};
