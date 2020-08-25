import { Component } from '@angular/core';
import { WeatherForecast } from '../models/weather-forecast';
import { WeatherForecastService } from '../services/weather-forecast.service';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(weatherForecastService: WeatherForecastService) {
    weatherForecastService.get().subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}
