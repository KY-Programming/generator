/* eslint-disable */
// tslint:disable

import { Signal } from "@angular/core";

export type Unwrapped<TValue> = TValue extends Signal<infer TSignalValue> ? Unwrapped<TSignalValue> : TValue extends Date ? TValue : TValue extends (infer TEntry)[] ? Unwrapped<TEntry>[] : TValue extends object ? { [TKey in keyof TValue]: Unwrapped<TValue[TKey]> } : TValue

// outputid:b574b782-3e0e-4f8c-8378-106e837ed00d
