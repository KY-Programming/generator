'use strict';

// Installs npm packages on demand, so a fresh clone validates without a manual install.
//
// Two roots use this:
//
//   Tests            the packages the generated output imports, shared by the tests and by the
//                    examples that write loose models
//   <example>/ClientApp   an example that ships a client app of its own is checked against that app's
//                    packages instead, so it compiles against the toolchain it actually uses
//
// npm ci is used when a package-lock.json is checked in, so the lock decides the versions.

const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');

/** Installs into `root` unless node_modules is already there. Returns null on success, else the error. */
function ensurePackages(root) {
    if (fs.existsSync(path.join(root, 'node_modules'))) {
        return null;
    }
    if (!fs.existsSync(path.join(root, 'package.json'))) {
        return `${root} has no package.json`;
    }

    console.log(`Installing the npm packages of ${root}...`);
    const command = fs.existsSync(path.join(root, 'package-lock.json')) ? 'ci' : 'install';
    // npm is a shell script (npm.cmd on Windows), so it needs a shell to start.
    const npm = spawnSync('npm', [command, '--no-audit', '--no-fund'], { cwd: root, stdio: 'inherit', shell: true });
    return npm.status === 0 ? null : `npm ${command} exited with ${npm.status}`;
}

module.exports = { ensurePackages };
