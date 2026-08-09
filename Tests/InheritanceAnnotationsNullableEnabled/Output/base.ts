/* eslint-disable */
// tslint:disable

export class Base {
    public stringField: string = "";
    public stringProperty: string = "";

    public constructor(init?: Partial<Base>) {
        Object.assign(this, init);
    }
}

// outputid:a1e4c7d2-3b5f-4e88-9c10-5d2f7a6b4e31
