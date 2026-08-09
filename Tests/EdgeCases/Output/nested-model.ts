/* eslint-disable */
// tslint:disable

export class NestedModel {
    public nestedProperty: string = "";

    public constructor(init?: Partial<NestedModel>) {
        Object.assign(this, init);
    }
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
