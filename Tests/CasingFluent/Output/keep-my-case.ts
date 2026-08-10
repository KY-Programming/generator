/* eslint-disable */
// tslint:disable

export class KeepMyCase {
    public alllower: string = "";
    public ALLUPPER: string = "";
    public PascalCase: string = "";
    public camelCase: string = "";
    public snake_case: string = "";
    public UPPER_SNAKE_CASE: string = "";
    public S1: string = "";

    public constructor(init?: Partial<KeepMyCase>) {
        Object.assign(this, init);
    }
}

// outputid:f7601c4b-055c-4bd5-a087-b514d1dde023
