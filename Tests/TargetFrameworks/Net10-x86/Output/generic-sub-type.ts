/* eslint-disable */
// tslint:disable

export class GenericSubType<TOne, TTwo> {
    public single?: TOne;
    public enumerable: TOne[] = [];
    public list: TTwo[] = [];
    public stringList: string[] = [];

    public constructor(init?: Partial<GenericSubType<TOne, TTwo>>) {
        Object.assign(this, init);
    }
}

// outputid:8c8a1d3a-034b-4633-bf8c-aa6ac39af861
