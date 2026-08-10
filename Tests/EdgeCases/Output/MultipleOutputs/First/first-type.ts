/* eslint-disable */
// tslint:disable

import { MultipleOutputsSubType } from "./multiple-outputs-sub-type";

export class FirstType {
    public stringProperty: string = "";
    public subTypeProperty: MultipleOutputsSubType | undefined;

    public constructor(init?: Partial<FirstType>) {
        Object.assign(this, init);
    }
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
