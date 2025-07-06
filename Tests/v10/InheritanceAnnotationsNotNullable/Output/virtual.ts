/* eslint-disable */
// tslint:disable

export class Virtual {
    public stringProperty?: string;
    public virtualProperty?: string;

    public constructor(init?: Partial<Virtual>) {
        Object.assign(this, init);
    }
}

// outputid:605e91d7-ee13-4f1d-9b92-845bb3ace852
