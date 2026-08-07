import { Component, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { SpecialWeatherForecast } from '@my-lib/models';
import { CustomWeatherForecast } from './models/custom-weather-forecast';
import { WeatherForecastApiService } from './services/weather-forecast-api.service';

/**
 * Both calls below are typed by [GenerateMethod] / [GenerateImport] rather than by the C#
 * return type of the action:
 *   - get() is declared as CustomWeatherForecast[], a hand written type in this app, so
 *     additionalProperty is available without any cast.
 *   - specialGet() is declared as SpecialWeatherForecast[], imported from "@my-lib/models".
 *   - producesGet() hits an action typed as IActionResult; ASP.NET's own [Produces] is what
 *     gives it a real return type here, with no KY.Generator attribute involved.
 */
@Component({
  selector: 'app-root',
  imports: [DatePipe],
  templateUrl: './app.html'
})
export class App {
  private readonly service = inject(WeatherForecastApiService);

  protected readonly forecasts = signal<CustomWeatherForecast[]>([]);
  protected readonly special = signal<SpecialWeatherForecast[]>([]);
  protected readonly produces = signal<CustomWeatherForecast[]>([]);
  protected readonly error = signal<string | undefined>(undefined);

  public constructor() {
    this.service.get().subscribe({
      next: (result) => this.forecasts.set(result),
      error: (error: unknown) => this.error.set(String(error))
    });
    this.service.specialGet().subscribe({
      next: (result) => this.special.set(result),
      error: (error: unknown) => this.error.set(String(error))
    });
    this.service.producesGet().subscribe({
      next: (result) => this.produces.set(result),
      error: (error: unknown) => this.error.set(String(error))
    });
  }
}
