'use strict';

// The command the generator runs through [assembly: RunAtSuccess]. It leaves a marker behind so validate.js can
// tell whether it ran at all, and when - the whole point of the hook is that it runs after the last file is
// written, which a marker written before the output would not prove.

const fs = require('fs');
const path = require('path');

const marker = path.join(__dirname, 'obj', 'run-at-success.marker');
fs.mkdirSync(path.dirname(marker), { recursive: true });
fs.writeFileSync(marker, new Date().toISOString() + '\n');
console.log(`run-at-success: wrote ${marker}`);
