/* eslint-disable */
// tslint:disable

import { Base } from "./base";

export class SealedDerived extends Base {
    public intProperty: number = 0;

    public constructor(init?: Partial<SealedDerived>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:a1e4c7d2-3b5f-4e88-9c10-5d2f7a6b4e31
