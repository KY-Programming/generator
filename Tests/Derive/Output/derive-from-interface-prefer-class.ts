/* eslint-disable */
// tslint:disable

import { IBaseInterface } from "./base-interface.interface";

export class DeriveFromInterfacePreferClass implements IBaseInterface {
    public stringProperty?: string | undefined;

    public constructor(init?: Partial<DeriveFromInterfacePreferClass>) {
        Object.assign(this, init);
    }
}

// outputid:e818f5ca-e427-4c1c-baf2-38476bfa9665
