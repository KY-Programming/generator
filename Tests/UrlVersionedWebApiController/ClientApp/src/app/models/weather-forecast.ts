/* eslint-disable */
// tslint:disable

export class WeatherForecast {
    public date: Date;
    public temperatureC: number;
    public temperatureF: number;
    public summary: string;

    public constructor(init?: Partial<WeatherForecast>) {
        Object.assign(this, init);
    }
}

// outputid:8f3ba999-d4ad-439a-8930-275b772addc3
