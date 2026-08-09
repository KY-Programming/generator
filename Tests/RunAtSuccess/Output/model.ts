/* eslint-disable */
// tslint:disable

export class Model {
    public stringProperty: string = "";

    public constructor(init?: Partial<Model>) {
        Object.assign(this, init);
    }
}

// outputid:98a5ff19-8b2d-4ab0-93ee-a05133c49936
