/* eslint-disable */
// tslint:disable

import { IInterface } from "./interface.interface";

export class CMyClassWithIInterface implements IInterface {
    public constructor(init?: Partial<CMyClassWithIInterface>) {
        Object.assign(this, init);
    }
}

// outputid:95ca21b8-f1ff-4cc7-b96e-63d5a90970c3
