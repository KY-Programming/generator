/* eslint-disable */
// tslint:disable

// Pulled into the first and the second output folder, once by each type that exposes it.
export class MultipleOutputsSubType {
    public stringProperty: string = "";

    public constructor(init?: Partial<MultipleOutputsSubType>) {
        Object.assign(this, init);
    }
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
