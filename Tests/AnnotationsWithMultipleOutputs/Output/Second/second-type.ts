/* eslint-disable */
// tslint:disable

import { SubType } from "./sub-type";

export class SecondType {
    public stringProperty: string;
    public subTypeProperty: SubType;

    public constructor(init: Partial<SecondType> = undefined) {
        Object.assign(this, init);
    }
}

// outputid:352b1947-a770-4737-be18-608a78c130ad
