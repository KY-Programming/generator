/* eslint-disable */
// tslint:disable

import { EdgeCase1SubType } from "./edge-case-1-sub-type";

export class EdgeCase1 extends EdgeCase1SubType<string> {
    public constructor(init?: Partial<EdgeCase1>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
