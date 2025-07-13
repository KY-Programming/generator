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

// outputid:605e91d7-ee13-4f1d-9b92-845bb3ace852
