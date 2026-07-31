/* eslint-disable */
// tslint:disable

export class StrictClass {
    public stringProperty?: string;
    public requiredStringProperty: string | undefined;
    public requiredStringWithDefaultProperty: string | undefined;
    public intProperty: number = 0;
    public nullableIntProperty?: number;
    public requiredNullableIntProperty: number | undefined;

    public constructor(init?: Partial<StrictClass>) {
        Object.assign(this, init);
    }
}

// outputid:5bfb8c6c-eb36-4be2-ab49-5a01780aa03e
