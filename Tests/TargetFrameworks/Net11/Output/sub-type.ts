/* eslint-disable */

export class SubType {
    public property: string = "";

    public constructor(init?: Partial<SubType>) {
        Object.assign(this, init);
    }
}

// outputid:cc38a545-0dd1-42bf-962c-e5b63c752c93
