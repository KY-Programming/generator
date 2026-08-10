/* eslint-disable */
// tslint:disable

import { DateModel } from "./date-model";

export class DateModelWrapperWithDate {
    public id: string = "00000000-0000-0000-0000-000000000000";
    public date: Date = new Date(0);
    public model: DateModel | undefined;

    public constructor(init?: Partial<DateModelWrapperWithDate>) {
        Object.assign(this, init);
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
