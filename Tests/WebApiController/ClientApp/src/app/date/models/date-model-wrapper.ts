/* eslint-disable */
// tslint:disable

import { DateModel } from "./date-model";

export class DateModelWrapper {
    public id?: string;
    public model?: DateModel;

    public constructor(init?: Partial<DateModelWrapper>) {
        Object.assign(this, init);
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
