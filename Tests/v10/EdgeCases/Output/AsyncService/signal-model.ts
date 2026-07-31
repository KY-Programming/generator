/* eslint-disable */
// tslint:disable

import { SubModel } from "./sub-model";
import { WritableSignal } from "@angular/core";

// Model with signals. The generated service has to wrap it after every read and unwrap it before every write
export interface SignalModel {
    text: WritableSignal<string>;
    number: WritableSignal<number>;
    timestamp: WritableSignal<Date>;
    optionalText: WritableSignal<string | undefined>;
    texts: WritableSignal<string[]>;
    sub: WritableSignal<SubModel | undefined>;
    subs: WritableSignal<SubModel[]>;
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
