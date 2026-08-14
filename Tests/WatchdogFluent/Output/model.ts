/* eslint-disable */
// tslint:disable

export class Model {
    public stringProperty: string = "";

    public constructor(init?: Partial<Model>) {
        Object.assign(this, init);
    }
}

// outputid:5b1f2c74-8f3a-4d16-9a71-2c58e0b4d9c1
