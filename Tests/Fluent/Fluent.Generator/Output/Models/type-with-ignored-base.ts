/* eslint-disable */
// tslint:disable

export class TypeWithIgnoredBase {
    public property: string;

    public constructor(init?: Partial<TypeWithIgnoredBase>) {
        Object.assign(this, init);
    }
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
