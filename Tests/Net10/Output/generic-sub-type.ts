/* eslint-disable */
// tslint:disable

export class GenericSubType<TOne, TTwo> {
    public single?: TOne;
    public single2: string;
    public enumerable: TOne[];
    public list: TTwo[];
    public stringList: string[];

    public constructor(init?: Partial<GenericSubType<TOne, TTwo>>) {
        Object.assign(this, init);
    }
}

// outputid:3fe25a27-4f60-4718-8fb2-be47996315d8
