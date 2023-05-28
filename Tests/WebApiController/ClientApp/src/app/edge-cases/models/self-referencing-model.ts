/* eslint-disable */
// tslint:disable

export class SelfReferencingModel {
    public name?: string;
    public children?: SelfReferencingModel[];

    public constructor(init?: Partial<SelfReferencingModel>) {
        Object.assign(this, init);
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
