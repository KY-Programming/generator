/* eslint-disable */
// tslint:disable

import { DateModel } from "./date-model";

export class DateModelWrapperWithDate {
    public id: string;
    public date: Date;
    public model: DateModel;

    public constructor(init?: Partial<DateModelWrapperWithDate>) {
        Object.assign(this, init);
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
