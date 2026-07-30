/* eslint-disable */
// tslint:disable

import { ISubInterface } from "./sub-interface.interface";

export interface IGeneratedInterface {
    name: string;
    value: number;
    nullableName?: string;
    items: string[];
    sub: ISubInterface | undefined;
}

// outputid:3abd506b-c448-4711-85a9-e6ac03f4d1ca
