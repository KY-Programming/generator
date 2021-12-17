/* eslint-disable */
// tslint:disable

import { CaseMe } from "./case-me";
import { KeepMyCase } from "./keep-my-case";

export class MixedCaseing {
    public caseMe: CaseMe;
    public keepMyCase: KeepMyCase;

    public constructor(init?: Partial<MixedCaseing>) {
        Object.assign(this, init);
    }
}

// outputid:f7601c4b-055c-4bd5-a087-b514d1dde023
