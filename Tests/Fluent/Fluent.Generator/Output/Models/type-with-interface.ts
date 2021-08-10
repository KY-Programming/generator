/* eslint-disable */
// tslint:disable

import { IInterface } from "./interface.interface";

export class TypeWithInterface implements IInterface {
    public property: string;

    public constructor(init?: Partial<TypeWithInterface>) {
        Object.assign(this, init);
    }
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
