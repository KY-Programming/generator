/* eslint-disable */
// tslint:disable

import { WritableSignal } from "@angular/core";

// Nested model. Inherits the signals from the model that uses it and gets its own wrap/unwrap methods
export interface SubModel {
    name: WritableSignal<string>;
    changed: WritableSignal<Date>;
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
