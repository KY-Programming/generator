/* eslint-disable */
// tslint:disable

import { Virtual } from "./virtual";

export class DerivedFromVirtual extends Virtual {
    public intProperty: number = 0;

    public constructor(init?: Partial<DerivedFromVirtual>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:5d394855-3af8-4bb7-be77-6fe980365c9a
