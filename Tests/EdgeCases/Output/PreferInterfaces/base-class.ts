/* eslint-disable */
// tslint:disable

// Carries a shadowed and a virtual member, so member resolution along the chain is covered in both modes.
export class BaseClass {
    public newStringProperty: string = "";
    public virtualStringProperty: string = "";

    public constructor(init?: Partial<BaseClass>) {
        Object.assign(this, init);
    }
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
