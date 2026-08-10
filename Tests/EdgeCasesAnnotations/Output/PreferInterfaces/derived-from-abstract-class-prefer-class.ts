/* eslint-disable */
// tslint:disable

import { AbstractType } from "./abstract-type";

export class DerivedFromAbstractClassPreferClass extends AbstractType {
    public stringProperty: string = "";

    public constructor(init?: Partial<DerivedFromAbstractClassPreferClass>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
