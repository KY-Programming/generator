'use strict';

// Asserts what [GenerateCsharpModel] promises, and compiles the result. The build does not compile it:
// the Builder deletes the generated files before it and the annotation pass writes them after the
// compiler ran - see Tests/Shared/Scripts/validate-csharp.js.
//
// Checked here:
//   - CustomerDto and the ContactDto it holds are written as Customer.cs and Contact.cs
//   - both land in Output/, not in the project root (the relative path reaches the write command)
//   - SubTypesOnly is not written, only the SupplierDto it holds (onlySubTypes reaches the read command)
//   - everything generated is C# in the namespace of the type it was read from, and compiles
//
// The last line of stdout is the machine readable result - the Builder reads that, not the exit code:
//
//     {"state":"passed","errors":0,"validated":3}

const fs = require('fs');
const path = require('path');

const OUTPUT = path.join(__dirname, 'Output');
const NAMESPACE = 'CsharpModelAnnotations.Source';

// The Dto suffix is dropped by [GenerateClass(Replace = "Dto")] - without it the generated types would
// collide with the sources they were read from, which sit in the same assembly.
const EXPECTED = ['Contact.cs', 'Customer.cs', 'Supplier.cs'];

function fail(message) {
    console.log(`VALIDATION FAILED: ${message}`);
    console.log(JSON.stringify({ state: 'failed', errors: 1, validated: 0 }));
    process.exit(1);
}

function declaresNamespace(content) {
    // Both the block and the file scoped form count - which one is written is not what this test is about.
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

    // SubTypesOnly.cs would be the onlySubTypes regression: the flag has to reach the read command, the
    // decorated type itself must not be written. Any other extra file is a regression as well.
    const unexpected = generated.filter(name => !EXPECTED.includes(name));
    if (unexpected.length > 0) {
        fail(`Output/ contains unexpected file(s): ${unexpected.join(', ')}.`);
    }

    // The relative path regression: without it the models are written to the output root, which is the
    // project directory - next to AssemblyInfo.cs instead of into Output/.
    const atRoot = fs.readdirSync(__dirname).filter(name => name.endsWith('.cs') && name !== 'AssemblyInfo.cs');
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
