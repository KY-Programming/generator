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

// outputid:627408ca-a818-4326-b843-415f5bbfb028
