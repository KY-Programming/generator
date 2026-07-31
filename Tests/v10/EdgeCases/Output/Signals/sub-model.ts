/* eslint-disable */
// tslint:disable

import { signal } from "@angular/core";
import { WritableSignal } from "@angular/core";

// Nested model that is generated with signals too
export class SubModel {
    public name: WritableSignal<string> = signal("");

    public constructor(init?: Partial<SubModel>) {
        Object.assign(this, init);
    }
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
