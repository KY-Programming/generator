import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { WeatherForecast } from './models/weather-forecast';
import { WeatherForecastService } from './services/weather-forecast.service';

@Component({
  selector: 'app-root',
  imports: [DatePipe],
  templateUrl: './app.html'
})
export class App {
  private readonly service = inject(WeatherForecastService);

  protected readonly forecasts = signal<WeatherForecast[]>([]);
  protected readonly error = signal<string | undefined>(undefined);

  public constructor() {
    this.load();
  }

  protected load(): void {
    this.service.get().subscribe({
      next: forecasts => {
        this.forecasts.set(forecasts);
        this.error.set(undefined);
      },
      error: (error: unknown) => this.error.set(`${error}`)
    });
  }

  protected add(): void {
    const entry = new WeatherForecast({ date: new Date(), temperatureC: 21, summary: 'Mild' });
    this.service.post(entry).subscribe({
      next: () => this.load(),
      error: (error: unknown) => this.error.set(`${error}`)
    });
  }

  protected remove(forecast: WeatherForecast): void {
    this.service.delete(forecast.summary).subscribe({
      next: () => this.load(),
      error: (error: unknown) => this.error.set(`${error}`)
    });
  }
}
