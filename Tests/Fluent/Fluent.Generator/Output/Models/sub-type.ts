// tslint:disable

export class SubType {
    public property: string;

    public constructor(init: Partial<SubType> = undefined) {
        Object.assign(this, init);
    }
}

// outputid:cabb1031-51d3-4479-98b6-c8c0321d279a