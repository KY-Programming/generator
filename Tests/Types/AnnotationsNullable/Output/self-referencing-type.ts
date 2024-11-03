/* eslint-disable */
// tslint:disable

export class SelfReferencingType {
    public stringProperty?: string | undefined;
    public selfProperty: SelfReferencingType;
    public selfList?: SelfReferencingType[] | undefined;

    public constructor(init?: Partial<SelfReferencingType>) {
        Object.assign(this, init);
    }
}

// outputid:f13dd313-1bd6-4a8e-9b3f-d6266d3a25ff
