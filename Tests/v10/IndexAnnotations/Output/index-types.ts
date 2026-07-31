/* eslint-disable */
// tslint:disable

import { IndexSubType } from "./index-sub-type";

export class IndexTypes {
    public stringProperty: string = "";
    public subType: IndexSubType | undefined;

    public constructor(init?: Partial<IndexTypes>) {
        Object.assign(this, init);
    }
}

// outputid:c3a6e9f4-5d71-40aa-9e32-7f4b9c8d6a53
