/* eslint-disable */
// tslint:disable

export class CasingModel {
    public alllower: string;
    public ALLUPPER: string;
    public PascalCase: string;
    public camelCase: string;
    public snake_case: string;
    public UPPER_SNAKE_CASE: string;

    public constructor(init?: Partial<CasingModel>) {
        Object.assign(this, init);
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
