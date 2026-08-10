/* eslint-disable */
// tslint:disable

import { ISubInterface } from "./sub-interface.interface";

// An interface as the generation source. Everywhere else in the suite interfaces are only an output style
// (GeneratePreferInterfaces); here the annotated C# type itself is an interface.
export interface IGeneratedInterface {
    name: string;
    value: number;
    nullableName?: string;
    items: string[];
    sub: ISubInterface | undefined;
}

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
