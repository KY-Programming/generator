/* eslint-disable */
// tslint:disable

import { IBaseInterface } from "./base-interface.interface";

export class DeriveFromInterfacePreferClass implements IBaseInterface {
    public stringProperty: string = "";

    public constructor(init?: Partial<DeriveFromInterfacePreferClass>) {
        Object.assign(this, init);
    }
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
