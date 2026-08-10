/* eslint-disable */
// tslint:disable

import { BaseClass } from "./base-class";

export class DerivedFromClassPreferClass extends BaseClass {
    public stringProperty: string = "";

    public constructor(init?: Partial<DerivedFromClassPreferClass>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
