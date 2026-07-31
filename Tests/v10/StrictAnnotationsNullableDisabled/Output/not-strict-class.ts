/* eslint-disable */
// tslint:disable

export class NotStrictClass {
    public stringProperty?: string;
    public requiredStringProperty: string;
    public intProperty: number;
    public nullableIntProperty?: number;
    public requiredNullableIntProperty: number;

    public constructor(init?: Partial<NotStrictClass>) {
        Object.assign(this, init);
    }
}

// outputid:5bfb8c6c-eb36-4be2-ab49-5a01780aa03e
