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

// outputid:b41f9d6c-7e02-4a35-9f18-6c3d5a2e4471
