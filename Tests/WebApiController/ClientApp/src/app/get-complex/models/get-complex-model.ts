/* eslint-disable */
// tslint:disable

import { GetComplexModelService } from "./get-complex-model-service";

export class GetComplexModel {
    public property?: string;
    public service?: GetComplexModelService;
    public services?: GetComplexModelService[];

    public constructor(init?: Partial<GetComplexModel>) {
        Object.assign(this, init);
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
