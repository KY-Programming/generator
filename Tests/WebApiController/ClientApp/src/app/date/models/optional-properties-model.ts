/* eslint-disable */
// tslint:disable

export class OptionalPropertiesModel {
    public requiredString: string;
    public requiredNotNullableString: string = "";
    public requiredInt: number;
    public optionalString?: string;
    public optionalInt?: number;

    public constructor(init?: Partial<OptionalPropertiesModel>) {
        Object.assign(this, init);
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
