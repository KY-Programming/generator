/* eslint-disable */
// tslint:disable

import { IGeneratedInterface } from "./generated-interface.interface";
import { ISubInterface } from "./sub-interface.interface";

// A class implementing the annotated interface. It is generated independently and must not collide
// with the interface file.
export interface ImplementingClass extends IGeneratedInterface {
    name: string;
    value: number;
    nullableName?: string;
    items: string[];
    sub: ISubInterface | undefined;
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
