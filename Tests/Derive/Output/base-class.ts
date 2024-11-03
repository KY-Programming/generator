/* eslint-disable */
// tslint:disable

export class BaseClass {
    public newStringProperty?: string;
    public virtualStringProperty?: string;

    public constructor(init?: Partial<BaseClass>) {
        Object.assign(this, init);
    }
}

// outputid:e818f5ca-e427-4c1c-baf2-38476bfa9665
