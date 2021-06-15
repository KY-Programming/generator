import { Component, Inject } from '@angular/core';
import { WeatherForecastService } from '../services/weather-forecast.service';
import { WeatherForecast } from '../models/weather-forecast';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(
    service: WeatherForecastService
  ) {
    service.get().subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}
