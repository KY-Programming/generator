import { Component, OnInit } from '@angular/core';
import { WeatherForecast } from '../produces/models/weather-forecast';
import { VersionedApiService } from './services/versioned-api.service';

@Component({
  selector: 'app-versioned-api',
  templateUrl: './versioned-api.component.html',
  styleUrls: ['./versioned-api.component.css']
})
export class VersionedApiComponent implements OnInit {
  public models: WeatherForecast[];

  constructor(
    private readonly service: VersionedApiService
  ) { }

  ngOnInit() {
    this.service.get().subscribe(models => this.models = models);
  }

}
