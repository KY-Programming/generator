import { Component, OnDestroy, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ConnectionStatus } from './models/connection-status';
import { WeatherForecast } from './models/weather-forecast';
import { WeatherHubService } from './services/weather-hub.service';

/**
 * WeatherHubService is generated from WeatherHub by [GenerateAngularHub]. It owns the SignalR
 * connection and exposes the hub's client callback (Updated) as updated$, its own connection
 * state as status$, and the hub method Fetch() as fetch(). The reconnect delays come from
 * [GenerateWithRetry] on the hub.
 */
@Component({
  selector: 'app-root',
  imports: [DatePipe],
  templateUrl: './app.html'
})
export class App implements OnDestroy {
  private readonly hub = inject(WeatherHubService);

  protected readonly forecasts = signal<WeatherForecast[]>([]);
  protected readonly status = signal<string>('disconnected');

  protected readonly ConnectionStatus = ConnectionStatus;

  public constructor() {
    this.hub.status$.subscribe((status) => this.status.set(ConnectionStatus[status]));
    this.hub.updated$.subscribe((forecasts) => this.forecasts.set(forecasts));
    this.hub.connect().subscribe();
  }

  protected refresh(): void {
    this.hub.fetch().subscribe();
  }

  public ngOnDestroy(): void {
    this.hub.disconnect();
  }
}
