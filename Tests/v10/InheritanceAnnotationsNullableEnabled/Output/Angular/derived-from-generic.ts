/* eslint-disable */
// tslint:disable

import { GenericBase } from "./generic-base";

export class DerivedFromGeneric extends GenericBase<string> {
    public intProperty: number = 0;

    public constructor(init?: Partial<DerivedFromGeneric>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:a1e4c7d2-3b5f-4e88-9c10-5d2f7a6b4e31
