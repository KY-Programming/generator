// tslint:disable

export class WeatherForecast {
    public date: Date;
    public temperatureC: number;
    public temperatureF: number;
    public summary: string;

    public constructor(init: Partial<WeatherForecast> = undefined) {
        Object.assign(this, init);
    }
}