/* eslint-disable */
// tslint:disable

export class GenericResult<T> {
    public rows?: T[];
    public strings?: string[];

    public constructor(init?: Partial<GenericResult<T>>) {
        Object.assign(this, init);
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
