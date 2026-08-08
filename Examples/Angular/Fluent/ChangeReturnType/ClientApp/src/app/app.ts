import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { CustomWeatherForecast } from './models/custom-weather-forecast';
import { WeatherForecastService } from './services/weather-forecast.service';

/**
 * The controller action returns WeatherForecast, but SetMember(...).ReturnType("CustomWeatherForecast[]")
 * in the generator makes get() return CustomWeatherForecast[] instead, imported from the hand written
 * model in this app. That is why additionalProperty is available here without any cast.
 */
@Component({
  selector: 'app-root',
  imports: [DatePipe],
  templateUrl: './app.html'
})
export class App {
  private readonly service = inject(WeatherForecastService);

  protected readonly forecasts = signal<CustomWeatherForecast[]>([]);
  protected readonly error = signal<string | undefined>(undefined);

  public constructor() {
    this.service.get().subscribe({
      next: (result) => this.forecasts.set(result),
      error: (error: unknown) => this.error.set(String(error))
    });
  }
}
