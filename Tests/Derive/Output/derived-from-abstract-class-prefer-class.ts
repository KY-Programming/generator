/* eslint-disable */
// tslint:disable

import { AbstractType } from "./abstract-type";

export class DerivedFromAbstractClassPreferClass extends AbstractType {
    public stringProperty?: string | undefined;

    public constructor(init?: Partial<DerivedFromAbstractClassPreferClass>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:e818f5ca-e427-4c1c-baf2-38476bfa9665
