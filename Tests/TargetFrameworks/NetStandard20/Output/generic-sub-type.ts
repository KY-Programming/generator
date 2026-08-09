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

// outputid:4a02ee87-ce35-4761-a2fe-498c859c7974
