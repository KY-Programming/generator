/* eslint-disable */
// tslint:disable

import { SubModel } from "./sub-model";
import { WritableSignal } from "@angular/core";

export interface SignalModel {
    text: WritableSignal<string | undefined>;
    number: WritableSignal<number>;
    timestamp: WritableSignal<Date>;
    optionalText: WritableSignal<string | undefined>;
    texts: WritableSignal<string[] | undefined>;
    sub: WritableSignal<SubModel | undefined>;
    subs: WritableSignal<SubModel[] | undefined>;
}

// outputid:b574b782-3e0e-4f8c-8378-106e837ed00d
