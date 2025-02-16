/* eslint-disable */
// tslint:disable

export class StrictClass {
    public stringProperty: string;
    public nullableStringProperty?: string | undefined;
    public requiredNullableStringProperty: string | undefined;
    public intProperty: number = 0;
    public nullableIntProperty?: number | undefined;
    public requiredNullableIntProperty: number | undefined;

    public constructor(init?: Partial<StrictClass>) {
        Object.assign(this, init);
    }
}

// outputid:f13dd313-1bd6-4a8e-9b3f-d6266d3a25ff
