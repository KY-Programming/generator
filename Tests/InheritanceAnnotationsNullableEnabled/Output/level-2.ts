/* eslint-disable */
// tslint:disable

import { Level1 } from "./level-1";

export class Level2 extends Level1 {
    public level2Property: string = "";

    public constructor(init?: Partial<Level2>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:a1e4c7d2-3b5f-4e88-9c10-5d2f7a6b4e31
