/* eslint-disable */
// tslint:disable

export class WeatherForecast {
  public date: Date = new Date(0);
  public temperatureC: number = 0;
  public temperatureF: number = 0;
  public summary: string = '';

  public constructor(init?: Partial<WeatherForecast>) {
    Object.assign(this, init);
  }
}


// outputid:7d7037b1-21de-4798-a761-dcb57d990403
