/* eslint-disable */
// tslint:disable

import { Level1 } from "./level-1";

export class Level2 extends Level1 {
    public level2Property?: string;

    public constructor(init?: Partial<Level2>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:5d394855-3af8-4bb7-be77-6fe980365c9a
