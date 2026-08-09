/* eslint-disable */
// tslint:disable

import { WritableSignal } from "@angular/core";

// Every member has to be generated as signal. Optional members stay optional in the value type, but the member itself
// is always present e.g. optionalString: WritableSignal<string | undefined>
export interface SignalInterface {
    text: WritableSignal<string>;
    number: WritableSignal<number>;
    switch: WritableSignal<boolean>;
    timestamp: WritableSignal<Date>;
    optionalText: WritableSignal<string | undefined>;
    texts: WritableSignal<string[]>;
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
