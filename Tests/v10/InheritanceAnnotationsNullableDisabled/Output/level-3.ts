/* eslint-disable */
// tslint:disable

import { Level2 } from "./level-2";

export class Level3 extends Level2 {
    public level3Property?: string;

    public constructor(init?: Partial<Level3>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:5d394855-3af8-4bb7-be77-6fe980365c9a
