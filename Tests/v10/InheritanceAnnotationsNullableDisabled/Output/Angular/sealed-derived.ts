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

// outputid:5d394855-3af8-4bb7-be77-6fe980365c9a
