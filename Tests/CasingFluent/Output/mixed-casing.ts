/* eslint-disable */
// tslint:disable

import { CaseMe } from "./case-me";
import { KeepMyCase } from "./keep-my-case";

export class MixedCasing {
    public caseMe: CaseMe | undefined;
    public keepMyCase: KeepMyCase | undefined;

    public constructor(init?: Partial<MixedCasing>) {
        Object.assign(this, init);
    }
}

// outputid:f7601c4b-055c-4bd5-a087-b514d1dde023
