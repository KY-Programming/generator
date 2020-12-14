// tslint:disable

export class ThirdType {
    public stringProperty: string;

    public constructor(init: Partial<ThirdType> = undefined) {
        Object.assign(this, init);
    }
}

// outputid:352b1947-a770-4737-be18-608a78c130ad