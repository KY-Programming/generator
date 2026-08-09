/* eslint-disable */
// tslint:disable

import { CycleB } from "./cycle-b";

export class CycleA {
    public name?: string;
    public b?: CycleB;

    public constructor(init?: Partial<CycleA>) {
        Object.assign(this, init);
    }
}

// outputid:b2f5d8e3-4c60-4f99-8d21-6e3a8b7c5f42
