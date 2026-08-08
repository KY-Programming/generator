/* eslint-disable */
// tslint:disable

export interface GenericSubType<TOne, TTwo> {
    single?: TOne;
    single2: string;
    enumerable: TOne[];
    list: TTwo[];
    genericList: GenericSubType<TOne, TTwo>[];
    stringList: string[];
}

// outputid:d599f518-8c6c-4031-a89a-531c7e7fac42
