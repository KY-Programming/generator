import { DatePipe } from '@angular/common';
import { Component, OnDestroy, inject, signal } from '@angular/core';
import { ConnectionStatus } from './models/connection-status';
import { WeatherForecast } from './models/weather-forecast';
import { WeatherForecastHubService } from './services/weather-forecast-hub.service';

/**
 * WeatherForecastHubService is generated from WeatherForecastHub by Read().AspDotNet(x => x.FromHub<T>()).
 * It owns the SignalR connection and exposes the hub's client callback (Refreshed) as refreshed$,
 * its own connection state as status$, and the hub method Refresh() as refresh(). The generated
 * service has no default serviceUrl, so the app points it at the mapped hub route.
 */
@Component({
  selector: 'app-root',
  imports: [DatePipe],
  templateUrl: './app.html'
})
export class App implements OnDestroy {
  private readonly hub = inject(WeatherForecastHubService);

  protected readonly forecasts = signal<WeatherForecast[]>([]);
  protected readonly status = signal<string>('disconnected');

  public constructor() {
    this.hub.serviceUrl = `${document.baseURI}hub/weather`;
    this.hub.status$.subscribe((status) => this.status.set(ConnectionStatus[status]));
    this.hub.refreshed$.subscribe((forecasts) => this.forecasts.set(forecasts));
    this.hub.connect().subscribe();
  }

  protected refresh(): void {
    this.hub.refresh().subscribe();
  }

  public ngOnDestroy(): void {
    this.hub.disconnect();
  }
}
