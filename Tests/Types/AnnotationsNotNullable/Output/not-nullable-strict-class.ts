/* eslint-disable */
// tslint:disable

export class NotNullableStrictClass {
    public stringProperty?: string | undefined;
    public requiredStringProperty: string | undefined;
    public requiredStringWithDefaultProperty: string | undefined;
    public intProperty: number = 0;
    public nullableIntProperty?: number | undefined;
    public requiredNullableIntProperty: number | undefined;

    public constructor(init?: Partial<NotNullableStrictClass>) {
        Object.assign(this, init);
    }
}

// outputid:88210355-ddb5-4bf6-a426-81493e839d4e
