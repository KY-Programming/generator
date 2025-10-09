/* eslint-disable */
// tslint:disable

export class Base {
    public stringField?: string;
    public stringProperty?: string;

    public constructor(init?: Partial<Base>) {
        Object.assign(this, init);
    }
}

// outputid:605e91d7-ee13-4f1d-9b92-845bb3ace852
