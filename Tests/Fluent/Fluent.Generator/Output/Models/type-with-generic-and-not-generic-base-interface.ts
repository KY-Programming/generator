/* eslint-disable */
// tslint:disable

import { IGenericInterfaceWithNonGenericBase } from "./generic-interface-with-non-generic-base.interface";

export class TypeWithGenericAndNotGenericBaseInterface implements IGenericInterfaceWithNonGenericBase<string> {
    public property: string;
    public genericProperty: string;

    public constructor(init?: Partial<TypeWithGenericAndNotGenericBaseInterface>) {
        Object.assign(this, init);
    }
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
