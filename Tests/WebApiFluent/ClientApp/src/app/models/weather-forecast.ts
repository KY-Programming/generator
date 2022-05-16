/* eslint-disable */
// tslint:disable

export class WeatherForecast {
    public date?: Date;
    public temperatureC?: number;
    public temperatureF?: number;
    public summary?: string;

    public constructor(init?: Partial<WeatherForecast>) {
        Object.assign(this, init);
    }
}

// outputid:7d7037b1-21de-4798-a761-dcb57d990403
