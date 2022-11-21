/* eslint-disable */
// tslint:disable

export class WeatherForecast {
    public date?: Date = new Date(0);
    public temperatureC?: number = 0;
    public temperatureF?: number = 0;
    public summary?: string | undefined;

    public constructor(init?: Partial<WeatherForecast>) {
        Object.assign(this, init);
    }
}

// outputid:969bbdc9-df23-40ed-b6dd-702aa1fe7b03
