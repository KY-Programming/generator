'use strict';

// Starts the T-SQL server an example generates from and applies its schema, so the generation has a
// database to read even though the repository cannot hold one.
//
// Used as a Builder preparation script - it runs before the build and reports the same JSON contract a
// validation script does, as the last line of stdout:
//
//     {"state":"passed","errors":0,"validated":3}
//
// "validated" is the number of tables the schema ended up with, so project.md shows what the generation
// will find. A failure here means nothing was generated at all, which is why the Builder skips the build
// instead of reporting wrong output.
//
// Used from an example's own prepare.js:
//
//     require('../../../Tests/Shared/Scripts/mssql-container').run({
//         compose: path.join(__dirname, '..', 'docker-compose.yml'),
//         schema: path.join(__dirname, '..', 'schema.sql')
//     });
//
// The container belongs to the run, not to the machine: run() removes whatever is left over, creates it
// fresh, and teardown() - called from the example's cleanup.js - removes it again. Nothing is remembered
// between the two, because there is nothing to restore: after a run the container is gone either way.
//
// That also makes every run start from an empty server, so the generated output can never depend on what
// an earlier run left in the database.

const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');

// Has to match docker-compose.yml - the container is addressed directly for sqlcmd and the health poll.
const CONTAINER = 'ky-generator-tsql-example';

// Compose would otherwise name the project after the folder holding the compose file, so moving that file
// would silently change the network and volume names - and a teardown would no longer find what a former
// layout created. Pinned, so the folder structure is none of docker's business.
const PROJECT = 'ky-generator-tsql-example';
const PASSWORD = 'KyGenerator!2026';
const READY_TIMEOUT_SECONDS = 120;

// The image moved its command line tools between releases, so the path is probed instead of assumed.
const SQLCMD_CANDIDATES = [
    '/opt/mssql-tools18/bin/sqlcmd',
    '/opt/mssql-tools/bin/sqlcmd',
    '/usr/bin/sqlcmd'
];

function fail(message, code = 1) {
    console.error(message);
    console.log(JSON.stringify({ state: 'failed', errors: code, validated: 0 }));
    process.exit(0); // The JSON is the result - a non-zero exit would only duplicate it.
}

function pass(tables) {
    console.log(JSON.stringify({ state: 'passed', errors: 0, validated: tables }));
}

function docker(args, options = {}) {
    return spawnSync('docker', args, { encoding: 'utf8', ...options });
}

/** The whole script is synchronous, so the wait between two readiness polls has to be as well. */
function sleep(milliseconds) {
    Atomics.wait(new Int32Array(new SharedArrayBuffer(4)), 0, 0, milliseconds);
}

/**
 * Removes the container and everything compose created with it. "-v" drops the volumes too, so no
 * database file outlives the run.
 *
 * Used at both ends: before the container is created, to clear a leftover from a run that was killed
 * before its cleanup, and afterwards to remove this run's own. Removing something that is not there is
 * not an error for compose, so no check is needed before calling it.
 */
function removeContainer(compose) {
    return docker(['compose', '-p', PROJECT, '-f', compose, 'down', '-v']);
}

/** The daemon, not just the CLI - Docker Desktop being installed says nothing about it running. */
function assertDockerIsRunning() {
    const version = docker(['version', '--format', '{{.Server.Version}}']);
    if (version.error && version.error.code === 'ENOENT') {
        fail('docker was not found on the PATH. Install Docker Desktop to run this example.');
    }
    if (version.status !== 0) {
        fail('The Docker daemon is not reachable. Start Docker Desktop and run this example again.\n'
            + (version.stderr || '').trim());
    }
    console.log(`Docker ${(version.stdout || '').trim()}`);
}

function findSqlcmd() {
    for (const candidate of SQLCMD_CANDIDATES) {
        const result = docker(['exec', CONTAINER, 'test', '-x', candidate]);
        if (result.status === 0) {
            return candidate;
        }
    }
    return null;
}

/**
 * Runs a statement as sa. The server certificate is self signed, so -C (trust) is required for the
 * tools18 build; the older one ignores the flag.
 */
function query(sqlcmd, sql, database) {
    const args = ['exec', CONTAINER, sqlcmd, '-S', 'localhost', '-U', 'sa', '-P', PASSWORD, '-C', '-b', '-h', '-1', '-W'];
    if (database) {
        args.push('-d', database);
    }
    args.push('-Q', sql);
    return docker(args);
}

/** Polls until the server answers - a started container is not a started SQL Server. */
function waitUntilReady(sqlcmd) {
    const deadline = Date.now() + READY_TIMEOUT_SECONDS * 1000;
    let last = '';
    while (Date.now() < deadline) {
        const result = query(sqlcmd, 'SELECT 1');
        if (result.status === 0) {
            return;
        }
        last = ((result.stderr || '') + (result.stdout || '')).trim();
        sleep(2000);
    }
    fail(`SQL Server was not ready within ${READY_TIMEOUT_SECONDS}s.\n${last}`);
}

function run({ compose, schema }) {
    if (!fs.existsSync(compose)) {
        fail(`docker-compose.yml not found: ${compose}`, 2);
    }
    if (!fs.existsSync(schema)) {
        fail(`schema not found: ${schema}`, 2);
    }

    assertDockerIsRunning();

    // A run that was killed before its cleanup would leave one behind - it is dropped rather than reused,
    // so this run cannot inherit a database somebody else's schema was applied to.
    removeContainer(compose);

    console.log('Starting the T-SQL server');
    const up = docker(['compose', '-p', PROJECT, '-f', compose, 'up', '-d'], { stdio: ['ignore', 'inherit', 'pipe'] });
    if (up.status !== 0) {
        fail(`docker compose up failed.\n${(up.stderr || '').trim()}`);
    }

    const sqlcmd = findSqlcmd();
    if (!sqlcmd) {
        fail(`No sqlcmd found in the container. Looked in: ${SQLCMD_CANDIDATES.join(', ')}`);
    }

    waitUntilReady(sqlcmd);
    console.log('SQL Server is up');

    // The server is empty, so this is what the generation will find - nothing else can be in there.
    // Copied into the container and read with -i instead of being passed as a query: the "GO" batch
    // separators are a feature of the sqlcmd file reader, and CREATE DATABASE needs its own batch.
    const target = '/tmp/' + path.basename(schema);
    const copied = docker(['cp', schema, `${CONTAINER}:${target}`]);
    if (copied.status !== 0) {
        fail(`Copying ${path.basename(schema)} into the container failed.\n${(copied.stderr || '').trim()}`);
    }
    const applied = docker(['exec', CONTAINER, sqlcmd, '-S', 'localhost', '-U', 'sa', '-P', PASSWORD, '-C', '-b', '-i', target]);
    if (applied.status !== 0) {
        fail(`Applying ${path.basename(schema)} failed.\n${((applied.stderr || '') + (applied.stdout || '')).trim()}`);
    }
    console.log(`${path.basename(schema)} applied`);

    const counted = query(sqlcmd,
        "SET NOCOUNT ON; SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'",
        'KyGeneratorExample');
    const tables = parseInt((counted.stdout || '').trim(), 10);
    if (!Number.isInteger(tables) || tables === 0) {
        // No tables means the generation would silently produce nothing - that is a failed preparation.
        fail(`The database has no tables after applying the schema.\n${(counted.stdout || '').trim()}`);
    }

    console.log(`${tables} table(s) ready to read`);
    pass(tables);
}

/**
 * Removes the container the run created. Called from the example's cleanup.js, which the Builder runs
 * after everything else - also after a failed build or a preparation that never got as far as starting
 * anything, so this has to work no matter how much of run() happened.
 */
function teardown({ compose }) {
    if (!fs.existsSync(compose)) {
        fail(`docker-compose.yml not found: ${compose}`, 2);
    }

    // Without a daemon there is no container, so there is nothing that could have been left behind. This
    // is the normal path when the preparation already failed because Docker was not running.
    if (docker(['version', '--format', '{{.Server.Version}}']).status !== 0) {
        console.log('Docker is not running - nothing to remove.');
        pass(0);
        return;
    }

    const result = removeContainer(compose);
    if (result.status !== 0) {
        fail(`Removing the container failed.\n${(result.stderr || '').trim()}`);
    }
    console.log('Container removed');
    pass(0);
}

module.exports = { run, teardown, CONTAINER, PASSWORD };
