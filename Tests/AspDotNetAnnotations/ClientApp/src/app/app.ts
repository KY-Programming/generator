import { Component, inject, signal } from '@angular/core';
import { WeatherForecast } from './routed/models/weather-forecast';
import { RoutedService } from './routed/services/routed.service';

/**
 * One of the twenty generated services, wired up so the app has something to run. Every other generated
 * file is type-checked as well - the validation runs tsc over all of src, not only over what is reachable
 * from here.
 */
@Component({
  selector: 'app-root',
  templateUrl: './app.html'
})
export class App {
  private readonly service = inject(RoutedService);

  protected readonly forecasts = signal<WeatherForecast[]>([]);

  public constructor() {
    this.service.get().subscribe((result) => this.forecasts.set(result));
  }
}
