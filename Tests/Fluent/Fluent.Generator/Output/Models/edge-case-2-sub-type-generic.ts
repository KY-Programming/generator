/* eslint-disable */
// tslint:disable

export class EdgeCase2SubTypeGeneric<T> {
    public genericProperty: T;

    public constructor(init?: Partial<EdgeCase2SubTypeGeneric<T>>) {
        Object.assign(this, init);
    }
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
