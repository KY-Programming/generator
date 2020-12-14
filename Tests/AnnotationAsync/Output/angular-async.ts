// tslint:disable

export class AngularAsync {
    public property: string;

    public constructor(init: Partial<AngularAsync> = undefined) {
        Object.assign(this, init);
    }
}

// outputid:84eb1514-1fc9-499f-924d-4a93a076f40a