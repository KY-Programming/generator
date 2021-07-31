import { Component } from '@angular/core';
import { WeatherForecastApiService } from './services/weather-forecast-api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'strict';
  constructor(
    private readonly service: WeatherForecastApiService
  ) {}

}
