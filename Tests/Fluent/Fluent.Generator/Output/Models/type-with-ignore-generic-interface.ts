/* eslint-disable */
// tslint:disable

export class TypeWithIgnoreGenericInterface {
    public ignoredProperty?: string;

    public constructor(init?: Partial<TypeWithIgnoreGenericInterface>) {
        Object.assign(this, init);
    }
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
