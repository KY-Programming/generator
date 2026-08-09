/* eslint-disable */
// tslint:disable

export class StrictClass {
    public stringProperty: string = "";
    public nullableStringProperty?: string;
    public requiredNullableStringProperty: string | undefined;
    public intProperty: number = 0;
    public nullableIntProperty?: number;
    public requiredNullableIntProperty: number | undefined;

    public constructor(init?: Partial<StrictClass>) {
        Object.assign(this, init);
    }
}

// outputid:cde068c8-67d9-4d7c-a869-9fc28f6b7f72
