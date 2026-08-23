using KY.Generator;

// The generation runs in a process that outlives the build, so nothing that runs after the build knows when -
// or whether - it finished. These hooks do: they run once the generation is over and leave the marker behind
// that the Builder waits for before it verifies the output.
[assembly: RunAtSuccess("node ../Shared/Scripts/background-signal.js succeeded")]
[assembly: RunAtFailure("node ../Shared/Scripts/background-signal.js failed")]
