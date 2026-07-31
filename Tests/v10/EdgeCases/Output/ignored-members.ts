/* eslint-disable */
// tslint:disable

// Covers excluding single members and whole types from the generation.
export class IgnoredMembers {
    public untouchedProperty: string = "";

    public constructor(init?: Partial<IgnoredMembers>) {
        Object.assign(this, init);
    }
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
