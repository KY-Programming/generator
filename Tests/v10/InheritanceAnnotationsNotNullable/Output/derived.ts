/* eslint-disable */
// tslint:disable

import { Base } from "./base";

export class Derived extends Base {
    public constructor(init?: Partial<Derived>) {
        super();
        Object.assign(this, init);
    }
}

// outputid:605e91d7-ee13-4f1d-9b92-845bb3ace852
