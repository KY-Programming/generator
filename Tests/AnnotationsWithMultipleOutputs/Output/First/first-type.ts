// tslint:disable

import { SubType } from "./sub-type";

export class FirstType {
    public stringProperty: string;
    public subTypeProperty: SubType;

    public constructor(init: Partial<FirstType> = undefined) {
        Object.assign(this, init);
    }
}