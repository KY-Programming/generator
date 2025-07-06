/* eslint-disable */
// tslint:disable

import { Abstract } from "./abstract";

export class DerivedFromAbstract extends Abstract {
    public intProperty: number;

    public constructor(init?: Partial<DerivedFromAbstract>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:605e91d7-ee13-4f1d-9b92-845bb3ace852
