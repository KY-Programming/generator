/* eslint-disable */
// tslint:disable

export class DateArrayWrapper {
    public id: string = "00000000-0000-0000-0000-000000000000";
    public dates: Date[] = [];

    public constructor(init?: Partial<DateArrayWrapper>) {
        Object.assign(this, init);
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
