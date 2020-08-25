import { Component } from '@angular/core';
import { WeatherForecast } from '../models/weather-forecast';
import { WeatherHubService } from '../services/weather-hub.service';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(
    private readonly weatherHubService: WeatherHubService
  ) {
    this.weatherHubService.updated$.subscribe(forecasts => this.forecasts = forecasts);
    this.weatherHubService.connect();
  }

  public refresh(): void {
    this.weatherHubService.fetch();
  }
}
