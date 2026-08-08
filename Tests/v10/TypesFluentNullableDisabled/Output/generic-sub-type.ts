/* eslint-disable */
// tslint:disable

export interface GenericSubType<TOne, TTwo> {
    single?: TOne;
    single2?: string;
    enumerable?: TOne[];
    list?: TTwo[];
    genericList?: GenericSubType<TOne, TTwo>[];
    stringList?: string[];
}

// outputid:34250918-9be9-4645-baab-94eff779a14c
