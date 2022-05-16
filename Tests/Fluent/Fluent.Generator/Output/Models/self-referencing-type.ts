/* eslint-disable */
// tslint:disable

export class SelfReferencingType {
    public property?: string;
    public self?: SelfReferencingType;

    public constructor(init?: Partial<SelfReferencingType>) {
        Object.assign(this, init);
    }
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
