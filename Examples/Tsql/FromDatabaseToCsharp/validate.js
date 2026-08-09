'use strict';

// Compiles the generated models with the project they were generated for. The build itself does not
// prove they are valid: the annotation pass writes them after the compiler ran, so a broken model would
// only surface on the next build.

const path = require('path');

require('../../../Tests/Shared/Scripts/validate-csharp').run(path.join(__dirname, 'Output'));
