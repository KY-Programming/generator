/* eslint-disable */
// tslint:disable

import { Base } from "./base";

export class Derived extends Base {
    public constructor(init?: Partial<Derived>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:5d394855-3af8-4bb7-be77-6fe980365c9a
