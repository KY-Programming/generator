/* eslint-disable */
// tslint:disable

export class GenericBase<T> {
    public genericProperty?: T;
    public baseProperty?: string;

    public constructor(init?: Partial<GenericBase<T>>) {
        Object.assign(this, init);
    }
}

// outputid:5d394855-3af8-4bb7-be77-6fe980365c9a
