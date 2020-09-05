// tslint:disable

import { SubType } from "./sub-type";

export class SecondType {
    public stringProperty: string;
    public subTypeProperty: SubType;

    public constructor(init: Partial<SecondType> = undefined) {
        Object.assign(this, init);
    }
}