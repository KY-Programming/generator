/* eslint-disable */
// tslint:disable

import { EdgeCase2SubTypeGeneric } from "./edge-case-2-sub-type-generic";

export class EdgeCase2SubType extends EdgeCase2SubTypeGeneric<string> {
    public property: string;

    public constructor(init?: Partial<EdgeCase2SubType>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
