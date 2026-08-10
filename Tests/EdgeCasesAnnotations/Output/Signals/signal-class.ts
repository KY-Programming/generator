/* eslint-disable */
// tslint:disable

import { PlainModel } from "./plain-model";
import { SubModel } from "./sub-model";
import { signal } from "@angular/core";
import { WritableSignal } from "@angular/core";

// Every member has to be generated as signal. Optional members stay optional in the value type, but the member itself
// is always present e.g. optionalString: WritableSignal<string | undefined>
export class SignalClass {
    public text: WritableSignal<string> = signal("");
    public number: WritableSignal<number> = signal(0);
    public switch: WritableSignal<boolean> = signal(false);
    public timestamp: WritableSignal<Date> = signal(new Date(0));
    public optionalText: WritableSignal<string | undefined> = signal(undefined);
    public texts: WritableSignal<string[]> = signal([]);
    public sub: WritableSignal<SubModel | undefined> = signal(undefined);
    public subs: WritableSignal<SubModel[]> = signal([]);
    public plain: WritableSignal<PlainModel | undefined> = signal(undefined);

    public constructor(init?: Partial<SignalClass>) {
        Object.assign(this, init);
    }
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
