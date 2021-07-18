import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { WeatherForecastService } from '../services/weather-forecast.service';
import { WeatherForecast } from '../models/weather-forecast';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private readonly service: WeatherForecastService) {
    service.serviceUrl = baseUrl;
    service.get().subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}
