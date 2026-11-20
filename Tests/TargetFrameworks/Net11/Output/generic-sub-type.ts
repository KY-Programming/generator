/* eslint-disable */

export class GenericSubType<TOne, TTwo> {
    public single?: TOne;
    public enumerable: TOne[] = [];
    public list: TTwo[] = [];
    public stringList: string[] = [];

    public constructor(init?: Partial<GenericSubType<TOne, TTwo>>) {
        Object.assign(this, init);
    }
}

// outputid:cc38a545-0dd1-42bf-962c-e5b63c752c93
