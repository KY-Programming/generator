using KY.Generator;

// Unlike the other modules the watchdog is a single assembly - commands, module and fluent syntax all sit
// in here, so there is no second half to name. Without this line nothing loads the module and the
// "watchdog" command does not exist: a project only references this assembly, and a referenced assembly is
// searched for modules only when it says so.
[assembly: GenerateWith("KY.Generator.Watchdog", UseSameVersion = true)]
