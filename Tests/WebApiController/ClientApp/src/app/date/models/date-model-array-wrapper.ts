/* eslint-disable */
// tslint:disable

import { DateModel } from "./date-model";

export class DateModelArrayWrapper {
    public id: string;
    public models: DateModel[];

    public constructor(init: Partial<DateModelArrayWrapper> = undefined) {
        Object.assign(this, init);
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
