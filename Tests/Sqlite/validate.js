'use strict';

// The generated repositories are C#, so they are compiled rather than type-checked - together with the
// models they read and the Microsoft.Data.Sqlite reference they use. The Builder deletes them before it
// builds, so without this the build never sees them.

const path = require('path');

require('../Shared/Scripts/validate-csharp').run(path.join(__dirname, 'Output'));
