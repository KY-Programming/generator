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

// outputid:5d394855-3af8-4bb7-be77-6fe980365c9a
