'use strict';

// Guards [assembly: RunAtSuccess]: the command has to run, and it has to run after the generation, not somewhere
// in the middle of it. run-at-success.js leaves a marker behind, so both are checkable - the marker has to exist,
// and it has to be at least as new as the last generated file.
//
// Afterwards the generated TypeScript is type-checked as in every other project, so a green run here means the
// hook did not cost the generation anything.

const fs = require('fs');
const path = require('path');

const marker = path.join(__dirname, 'obj', 'run-at-success.marker');
const outputDirectory = path.join(__dirname, 'Output');

function fail(reason) {
    console.log(`VALIDATION FAILED: ${reason}`);
    console.log(JSON.stringify({ state: 'failed', errors: 1, validated: 0 }));
    process.exit(1);
}

if (!fs.existsSync(marker)) {
    fail('the run at success command did not run - no obj/run-at-success.marker.');
}

const generated = fs.existsSync(outputDirectory)
    ? fs.readdirSync(outputDirectory).filter(file => file.endsWith('.ts')).map(file => path.join(outputDirectory, file))
    : [];
if (generated.length === 0) {
    fail('nothing was generated, so the marker proves nothing.');
}

const markerWritten = fs.statSync(marker).mtimeMs;
const lastGenerated = Math.max(...generated.map(file => fs.statSync(file).mtimeMs));
if (markerWritten < lastGenerated) {
    fail(`the run at success command ran before the generation finished (marker ${new Date(markerWritten).toISOString()}, last file ${new Date(lastGenerated).toISOString()}).`);
}

console.log(`run at success: marker is ${Math.round(markerWritten - lastGenerated)} ms younger than the last of ${generated.length} generated file(s)`);

require('../Shared/Scripts/validate-generated').run(__dirname);
