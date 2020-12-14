// tslint:disable

export class CasingModel {
    public alllower: string;
    public allupper: string;
    public pascalCase: string;
    public camelCase: string;
    public snakeCase: string;
    public upperSnakeCase: string;

    public constructor(init: Partial<CasingModel> = undefined) {
        Object.assign(this, init);
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028