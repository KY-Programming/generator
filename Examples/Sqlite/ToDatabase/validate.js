'use strict';

// The repository is generated next to the hand written model instead of into an Output folder, so the
// project directory itself is scanned - only the files carrying an output id count as generated.

const path = require('path');

require('../../../Tests/Shared/Scripts/validate-csharp').run(__dirname);
