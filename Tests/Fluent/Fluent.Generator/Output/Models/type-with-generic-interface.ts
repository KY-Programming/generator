/* eslint-disable */
// tslint:disable

import { IGenericInterface } from "./generic-interface.interface";

export class TypeWithGenericInterface implements IGenericInterface<string> {
    public property: string;

    public constructor(init?: Partial<TypeWithGenericInterface>) {
        Object.assign(this, init);
    }
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
