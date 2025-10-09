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

// outputid:88210355-ddb5-4bf6-a426-81493e839d4e
