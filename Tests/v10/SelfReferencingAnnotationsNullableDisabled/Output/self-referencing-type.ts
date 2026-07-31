/* eslint-disable */
// tslint:disable

import { CycleA } from "./cycle-a";

export class SelfReferencingType {
    public stringProperty?: string;
    public selfProperty?: SelfReferencingType;
    public selfList?: SelfReferencingType[];
    public selfDictionary?: Record<string, SelfReferencingType>;
    public cycle?: CycleA;

    public constructor(init?: Partial<SelfReferencingType>) {
        Object.assign(this, init);
    }
}

// outputid:b2f5d8e3-4c60-4f99-8d21-6e3a8b7c5f42
