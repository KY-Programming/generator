/* eslint-disable */
// tslint:disable

import { BaseClass } from "./base-class";

export class DerivedFromClassPreferClass extends BaseClass {
    public stringProperty?: string | undefined;

    public constructor(init?: Partial<DerivedFromClassPreferClass>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:e818f5ca-e427-4c1c-baf2-38476bfa9665
