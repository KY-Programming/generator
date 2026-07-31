/* eslint-disable */
// tslint:disable

import { CycleA } from "./cycle-a";

export class CycleB {
    public name?: string;
    public a?: CycleA;

    public constructor(init?: Partial<CycleB>) {
        Object.assign(this, init);
    }
}

// outputid:b2f5d8e3-4c60-4f99-8d21-6e3a8b7c5f42
