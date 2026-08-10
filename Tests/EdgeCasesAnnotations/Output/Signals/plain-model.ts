/* eslint-disable */
// tslint:disable

import { signal } from "@angular/core";
import { WritableSignal } from "@angular/core";

// Model without the annotation. Inherits the signals from the model that uses it ()
export class PlainModel {
    public name: WritableSignal<string> = signal("");

    public constructor(init?: Partial<PlainModel>) {
        Object.assign(this, init);
    }
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
