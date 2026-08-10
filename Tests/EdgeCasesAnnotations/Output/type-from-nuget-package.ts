/* eslint-disable */
// tslint:disable

// The type exposes a member whose type comes from a NuGet package rather than from the project or the
// framework, so the assembly loader has to resolve System.Reactive out of the package folder before the
// type can be read at all.
export class TypeFromNugetPackage {
    public test: string = "";

    public constructor(init?: Partial<TypeFromNugetPackage>) {
        Object.assign(this, init);
    }
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
