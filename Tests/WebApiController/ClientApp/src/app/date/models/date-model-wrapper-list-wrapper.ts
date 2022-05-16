/* eslint-disable */
// tslint:disable

import { DateModelWrapper } from "./date-model-wrapper";

export class DateModelWrapperListWrapper {
    public list?: DateModelWrapper[];

    public constructor(init?: Partial<DateModelWrapperListWrapper>) {
        Object.assign(this, init);
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
