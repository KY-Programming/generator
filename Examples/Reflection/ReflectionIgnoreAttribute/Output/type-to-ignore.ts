// Hand written on purpose - TypeToIgnore carries [GenerateIgnore], so the generator never writes this
// file and never overwrites it. [GenerateImport] on TypeToRead points the import of its members here,
// which is what keeps a member of an ignored type working: its TypeScript side stays under your control.

export class TypeToIgnore {
    public stringProperty: string = "";
    public onlyOnTheTypeScriptSide: boolean = true;
}
