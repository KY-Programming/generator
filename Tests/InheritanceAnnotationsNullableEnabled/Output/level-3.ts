/* eslint-disable */
// tslint:disable

import { Level2 } from "./level-2";

export class Level3 extends Level2 {
    public level3Property: string = "";

    public constructor(init?: Partial<Level3>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:a1e4c7d2-3b5f-4e88-9c10-5d2f7a6b4e31
