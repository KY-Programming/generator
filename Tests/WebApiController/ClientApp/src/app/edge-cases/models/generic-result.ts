﻿/* eslint-disable */
// tslint:disable

export class GenericResult<T> {
    public rows: string[];

    public constructor(init: Partial<GenericResult<T>> = undefined) {
        Object.assign(this, init);
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
