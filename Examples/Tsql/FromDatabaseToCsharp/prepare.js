'use strict';

// Starts the T-SQL server this example generates from - the generation reads its schema during the
// build, so the server has to be up before it. Shared with the other T-SQL examples.

const path = require('path');

require('../../../Tests/Shared/Scripts/mssql-container').run({
    compose: path.join(__dirname, '..', 'Shared', 'docker-compose.yml'),
    schema: path.join(__dirname, '..', 'Shared', 'schema.sql')
});
