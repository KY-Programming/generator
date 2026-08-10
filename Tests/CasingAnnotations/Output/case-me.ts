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

// outputid:b41f9d6c-7e02-4a35-9f18-6c3d5a2e4471
