/* eslint-disable */
// tslint:disable

import { PlainModel } from "./plain-model";
import { SubModel } from "./sub-model";
import { WritableSignal } from "@angular/core";

export interface SignalModel {
    text: WritableSignal<string | undefined>;
    number: WritableSignal<number>;
    switch: WritableSignal<boolean>;
    timestamp: WritableSignal<Date>;
    optionalText: WritableSignal<string | undefined>;
    texts: WritableSignal<string[] | undefined>;
    sub: WritableSignal<SubModel | undefined>;
    subs: WritableSignal<SubModel[] | undefined>;
    plain: WritableSignal<PlainModel | undefined>;
}

// outputid:d5d865ec-d536-4507-ba20-bc9f405af26e
