/* eslint-disable */
// tslint:disable

export class Virtual {
    public stringProperty?: string;
    public virtualProperty?: string;

    public constructor(init?: Partial<Virtual>) {
        Object.assign(this, init);
    }
}

// outputid:5d394855-3af8-4bb7-be77-6fe980365c9a
