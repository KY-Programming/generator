/* eslint-disable */
// tslint:disable

export class DateModel {
    public id: string;
    public date: Date;

    public constructor(init: Partial<DateModel> = undefined) {
        Object.assign(this, init);
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
