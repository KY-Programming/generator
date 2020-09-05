// tslint:disable

export class ThirdType {
    public stringProperty: string;

    public constructor(init: Partial<ThirdType> = undefined) {
        Object.assign(this, init);
    }
}