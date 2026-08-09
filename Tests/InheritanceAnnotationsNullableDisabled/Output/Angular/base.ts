/* eslint-disable */
// tslint:disable

export class Base {
    public stringField?: string;
    public stringProperty?: string;

    public constructor(init?: Partial<Base>) {
        Object.assign(this, init);
    }
}

// outputid:5d394855-3af8-4bb7-be77-6fe980365c9a
