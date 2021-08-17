/* eslint-disable */
// tslint:disable

export class TypeWithIgnoreInterface {
    public ignoredProperty: string;

    public constructor(init?: Partial<TypeWithIgnoreInterface>) {
        Object.assign(this, init);
    }
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
