﻿/* eslint-disable */
// tslint:disable

import { Interface } from "./interface";

export class MyClassWithInterface implements Interface {
    public constructor(init?: Partial<MyClassWithInterface>) {
        Object.assign(this, init);
    }
}

// outputid:95ca21b8-f1ff-4cc7-b96e-63d5a90970c3
