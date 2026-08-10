/* eslint-disable */
// tslint:disable

export class CaseMe {
    public alllower: string = "";
    public allupper: string = "";
    public pascalCase: string = "";
    public camelCase: string = "";
    public snakeCase: string = "";
    public upperSnakeCase: string = "";
    public s1: string = "";

    public constructor(init?: Partial<CaseMe>) {
        Object.assign(this, init);
    }
}

// outputid:f7601c4b-055c-4bd5-a087-b514d1dde023
