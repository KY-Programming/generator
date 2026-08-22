'use strict';

// Reports the result of a generation that [GenerateInBackground] detached from the build. That process outlives
// the build, so whoever started it - the Builder - has no other way to tell when the output is complete, or
// whether it was ever written at all.
//
// The generator runs this from both of its end hooks, which fire after the last file is written:
//
//     [assembly: RunAtSuccess("node ../Shared/Scripts/background-signal.js succeeded")]
//     [assembly: RunAtFailure("node ../Shared/Scripts/background-signal.js failed")]
//
// The marker goes into the project's "obj", because that is wiped before every build - a leftover from the
// previous run would end the next wait immediately, and with the wrong answer. The hooks run in the project
// directory, so that is where the marker is written relative to.

const fs = require('fs');
const path = require('path');

const states = ['succeeded', 'failed'];
const state = process.argv[2];

if (!states.includes(state)) {
    console.error(`background-signal: expected one of ${states.join(', ')}, got '${state ?? ''}'`);
    process.exit(1);
}

const projectDirectory = process.cwd();
const marker = path.join(projectDirectory, 'obj', 'background-run.json');
fs.mkdirSync(path.dirname(marker), { recursive: true });
fs.writeFileSync(marker, JSON.stringify({ state, time: new Date().toISOString() }) + '\n');
console.log(`background-signal: ${state} - wrote ${marker}`);
