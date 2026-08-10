import { Component, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { WeatherForecast } from './models/weather-forecast';
import { WeatherForecastService } from './services/weather-forecast.service';

/**
 * Both the service and the model are generated from WeatherForecastController by the fluent entry point
 * in Generator.cs. The component only exists so the generated code is compiled and type-checked.
 */
@Component({
  selector: 'app-root',
  imports: [DatePipe],
  templateUrl: './app.html'
})
export class App {
  private readonly service = inject(WeatherForecastService);

  protected readonly forecasts = signal<WeatherForecast[]>([]);

  public constructor() {
    this.service.get().subscribe((result) => this.forecasts.set(result));
  }
}
