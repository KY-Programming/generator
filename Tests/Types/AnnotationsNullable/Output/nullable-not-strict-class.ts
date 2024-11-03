/* eslint-disable */
// tslint:disable

export class NullableNotStrictClass {
    public stringProperty: string;
    public nullableStringProperty?: string;
    public requiredNullableStringProperty: string;
    public intProperty: number;
    public nullableIntProperty?: number;
    public requiredNullableIntProperty: number;

    public constructor(init?: Partial<NullableNotStrictClass>) {
        Object.assign(this, init);
    }
}

// outputid:f13dd313-1bd6-4a8e-9b3f-d6266d3a25ff
