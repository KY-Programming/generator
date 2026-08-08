'use strict';

// This generator writes the same model twice into the sibling NonStrict project, once strict and once
// not. Both folders are checked against the shared non strict tsconfig - strict output is valid under
// it as well, so one run covers the pair.

const path = require('path');

require('../../../../Tests/Shared/Scripts/validate-files').run(path.join(__dirname, '..', 'NonStrict', 'Output'), { strict: false });
