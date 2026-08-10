/* eslint-disable */
// tslint:disable

import { EdgeCase1SubTypeGeneric } from "./edge-case-1-sub-type-generic";

export class EdgeCase1 extends EdgeCase1SubTypeGeneric<string> {
    public constructor(init?: Partial<EdgeCase1>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
