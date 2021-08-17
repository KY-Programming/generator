/* eslint-disable */
// tslint:disable

export class TypeWithGenericIgnoredBase {
    public property: string;

    public constructor(init?: Partial<TypeWithGenericIgnoredBase>) {
        Object.assign(this, init);
    }
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
