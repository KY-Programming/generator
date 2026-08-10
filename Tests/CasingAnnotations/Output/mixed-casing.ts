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

// outputid:b41f9d6c-7e02-4a35-9f18-6c3d5a2e4471
