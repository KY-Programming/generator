/* eslint-disable */
// tslint:disable

export class GenericBase<T> {
    public genericProperty?: T;
    public baseProperty: string = "";

    public constructor(init?: Partial<GenericBase<T>>) {
        Object.assign(this, init);
    }
}

// outputid:a1e4c7d2-3b5f-4e88-9c10-5d2f7a6b4e31
