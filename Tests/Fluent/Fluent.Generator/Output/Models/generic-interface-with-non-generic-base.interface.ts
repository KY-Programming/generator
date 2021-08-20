/* eslint-disable */
// tslint:disable

import { IInterface } from "./interface.interface";

export interface IGenericInterfaceWithNonGenericBase<T> extends IInterface {
    genericProperty: T;
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
