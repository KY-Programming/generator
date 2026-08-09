/* eslint-disable */
// tslint:disable

import { CycleA } from "./cycle-a";

export class CycleB {
    public name: string = "";
    public a?: CycleA;

    public constructor(init?: Partial<CycleB>) {
        Object.assign(this, init);
    }
}

// outputid:b84d2f96-fba8-416c-b067-f17f09809015
