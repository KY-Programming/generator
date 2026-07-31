/* eslint-disable */
// tslint:disable

import { IHasName } from "./has-name.interface";

export class ImplementsInterface implements IHasName {
    public name?: string;
    public intProperty: number = 0;

    public constructor(init?: Partial<ImplementsInterface>) {
        Object.assign(this, init);
    }
}

// outputid:5d394855-3af8-4bb7-be77-6fe980365c9a
