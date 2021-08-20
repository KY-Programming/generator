/* eslint-disable */
// tslint:disable

import { EdgeCase1SubTypeNonGeneric } from "./edge-case-1-sub-type-non-generic";

export class EdgeCase1SubType<T> extends EdgeCase1SubTypeNonGeneric {
    public genericProperty: T;

    public constructor(init?: Partial<EdgeCase1SubType<T>>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
