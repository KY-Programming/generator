/* eslint-disable */
// tslint:disable

// Covers the member level annotations: renaming a member, renaming via replace and overriding the
// generated type.
export class MemberTypes {
    public renamedField: string = "";
    public untouchedProperty: string = "";
    public renamedProperty: string = "";
    public niceNameProperty: string = "";
    public typeNameOverrideProperty: string | undefined;

    public constructor(init?: Partial<MemberTypes>) {
        Object.assign(this, init);
    }
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
