/* eslint-disable */
// tslint:disable

export class NotStrictClass {
    public stringProperty: string;
    public nullableStringProperty?: string;
    public requiredNullableStringProperty: string;
    public intProperty: number;
    public nullableIntProperty?: number;
    public requiredNullableIntProperty: number;

    public constructor(init?: Partial<NotStrictClass>) {
        Object.assign(this, init);
    }
}

// outputid:cde068c8-67d9-4d7c-a869-9fc28f6b7f72
