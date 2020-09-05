// tslint:disable

export class SubType {
    public stringProperty: string;

    public constructor(init: Partial<SubType> = undefined) {
        Object.assign(this, init);
    }
}