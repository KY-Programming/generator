/* eslint-disable */
// tslint:disable

import { Signal } from "@angular/core";

export type Unwrapped<TValue> = TValue extends Signal<infer TSignalValue> ? Unwrapped<TSignalValue> : TValue extends Date ? TValue : TValue extends (infer TEntry)[] ? Unwrapped<TEntry>[] : TValue extends object ? { [TKey in keyof TValue]: Unwrapped<TValue[TKey]> } : TValue

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
