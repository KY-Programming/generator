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

// outputid:8d02ce37-8013-4a57-a40d-4a10c8c65de2
