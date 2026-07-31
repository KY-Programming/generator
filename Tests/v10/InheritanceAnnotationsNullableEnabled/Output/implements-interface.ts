/* eslint-disable */
// tslint:disable

import { IHasName } from "./has-name.interface";

export class ImplementsInterface implements IHasName {
    public name: string = "";
    public intProperty: number = 0;

    public constructor(init?: Partial<ImplementsInterface>) {
        Object.assign(this, init);
    }
}

// outputid:a1e4c7d2-3b5f-4e88-9c10-5d2f7a6b4e31
