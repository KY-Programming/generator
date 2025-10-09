/* eslint-disable */
// tslint:disable

export class SelfReferencingType {
    public stringProperty: string = "";
    public selfProperty?: SelfReferencingType;
    public selfList: SelfReferencingType[] = [];

    public constructor(init?: Partial<SelfReferencingType>) {
        Object.assign(this, init);
    }
}

// outputid:b84d2f96-fba8-416c-b067-f17f09809015
