// tslint:disable

export class SubType {
    public property: string;

    public constructor(init: Partial<SubType> = undefined) {
        Object.assign(this, init);
    }
}

// outputid:f32fd57a-e5ce-4ae5-97bc-bbca02f65904
