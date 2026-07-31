/* eslint-disable */
// tslint:disable

import { Abstract } from "./abstract";

export class DerivedFromAbstract extends Abstract {
    public intProperty: number = 0;

    public constructor(init?: Partial<DerivedFromAbstract>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:5d394855-3af8-4bb7-be77-6fe980365c9a
