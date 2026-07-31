/* eslint-disable */
// tslint:disable

import { CycleB } from "./cycle-b";

export class CycleA {
    public name: string = "";
    public b?: CycleB;

    public constructor(init?: Partial<CycleA>) {
        Object.assign(this, init);
    }
}

// outputid:b84d2f96-fba8-416c-b067-f17f09809015
