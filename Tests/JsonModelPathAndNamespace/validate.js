'use strict';

// Asserts what the JSON Model("Output", "Configuration", "KY.Generator.Tests.Json") syntax promises,
// and compiles the result. The build does not compile it: the Builder deletes the generated files before
// it and the annotation pass writes them after the compiler ran - see
// Tests/Shared/Scripts/validate-csharp.js.
//
// Checked here:
//   - both models are written to Output/, not to the project root (the relative path is applied)
//   - both declare the namespace that was passed in (the namespace is applied)
//   - the root model is named after the argument, not after document.json (the name is applied)
//   - both compile, in the project they were generated for
//
// The last line of stdout is the machine readable result - the Builder reads that, not the exit code:
//
//     {"state":"passed","errors":0,"validated":2}
//
// "validated" is the number of generated files that were checked.

const fs = require('fs');
const path = require('path');

const OUTPUT = path.join(__dirname, 'Output');
const NAMESPACE = 'KY.Generator.Tests.Json';

// Configuration is the name passed to Model(...) - document.json would have given "Document".
// Endpoint keeps the name the reader derives from the property it came from.
const EXPECTED = ['Configuration.cs', 'Endpoint.cs'];

function fail(message) {
    console.log(`VALIDATION FAILED: ${message}`);
    console.log(JSON.stringify({ state: 'failed', errors: 1, validated: 0 }));
    process.exit(1);
}

function declaresNamespace(content) {
    // Both the block and the file scoped form count - which one is written depends on the language
    // version of the target project, and this test is not about that.
    return new RegExp(`namespace\\s+${NAMESPACE.replace(/\./g, '\\.')}\\s*[;{]`).test(content);
}

function main() {
    if (!fs.existsSync(OUTPUT)) {
        fail('Output/ does not exist - nothing was generated.');
    }

    const generated = fs.readdirSync(OUTPUT).filter(name => name.endsWith('.cs')).sort();
    const missing = EXPECTED.filter(name => !generated.includes(name));
    if (missing.length > 0) {
        fail(`Output/ is missing ${missing.join(', ')} - found ${generated.join(', ') || 'nothing'}.`);
    }
    const unexpected = generated.filter(name => !EXPECTED.includes(name));
    if (unexpected.length > 0) {
        fail(`Output/ contains unexpected file(s): ${unexpected.join(', ')}.`);
    }

    // The regression this guards: with the relative path dropped the models were written to the output
    // root, which is the project directory, right next to Generator.cs.
    const atRoot = fs.readdirSync(__dirname).filter(name => name.endsWith('.cs') && name !== 'Generator.cs');
    if (atRoot.length > 0) {
        fail(`${atRoot.join(', ')} was written to the project root instead of Output/.`);
    }

    for (const name of EXPECTED) {
        const content = fs.readFileSync(path.join(OUTPUT, name), 'utf8');
        if (!declaresNamespace(content)) {
            fail(`Output/${name} does not declare namespace ${NAMESPACE}.`);
        }
        const type = path.basename(name, '.cs');
        if (!new RegExp(`\\b(?:class|interface)\\s+${type}\\b`).test(content)) {
            fail(`Output/${name} does not declare ${type}.`);
        }
    }

    console.log(`${EXPECTED.length} generated file(s) in Output/, all in namespace ${NAMESPACE}.`);

    // The assertions above only read the files - this one proves they are valid C#.
    require('../Shared/Scripts/validate-csharp').run(OUTPUT);
}

main();
