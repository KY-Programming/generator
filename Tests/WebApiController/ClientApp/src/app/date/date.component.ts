import { Component, OnInit } from '@angular/core';
import { DateService } from './services/date.service';

@Component({
  selector: 'app-date',
  templateUrl: './date.component.html',
  styleUrls: ['./date.component.css']
})
export class DateComponent implements OnInit {
  public dates: Date[];
  public types: string[];
  public message: string;
  public isDate: boolean[];
  public result: unknown;

  constructor(
    private readonly dateService: DateService
  ) { }

  ngOnInit() {
    this.load();
  }

  public load(): void {
    this.message = 'Loading...'
    this.dateService.get().subscribe(date => {
      this.result = JSON.parse(JSON.stringify(date));
      this.dates = [date];
      this.types = [typeof date];
      this.isDate = [date instanceof Date];
      this.message = undefined;
    });
  }

  public loadArray(): void {
    this.message = 'Loading...'
    this.dateService.getArray().subscribe(dates => {
      this.result = JSON.parse(JSON.stringify(dates));
      this.dates = dates;
      this.types = dates.map(x => typeof x);
      this.isDate = dates.map(x => x instanceof Date);
      this.message = undefined;
    });
  }

  public loadList(): void {
    this.message = 'Loading...'
    this.dateService.getList().subscribe(dates => {
      this.result = JSON.parse(JSON.stringify(dates));
      this.dates = dates;
      this.types = dates.map(x => typeof x);
      this.isDate = dates.map(x => x instanceof Date);
      this.message = undefined;
    });
  }

  public loadEnumerable(): void {
    this.message = 'Loading...'
    this.dateService.getEnumerable().subscribe(dates => {
      this.result = JSON.parse(JSON.stringify(dates));
      this.dates = dates;
      this.types = dates.map(x => typeof x);
      this.isDate = dates.map(x => x instanceof Date);
      this.message = undefined;
    });
  }

  public loadModel(): void {
    this.message = 'Loading...'
    this.dateService.getComplex().subscribe(model => {
      this.result = JSON.parse(JSON.stringify(model));
      this.dates = [model.date];
      this.types = [typeof model.date];
      this.isDate = [model.date instanceof Date];
      this.message = undefined;
    });
  }

  public loadModelArray(): void {
    this.message = 'Loading...'
    this.dateService.getComplexArray().subscribe(models => {
      this.result = JSON.parse(JSON.stringify(models));
      this.dates = models.map(x => x.date);
      this.types = models.map(x => typeof x.date);
      this.isDate = models.map(x => x.date instanceof Date);
      this.message = undefined;
    });
  }

  public loadModelList(): void {
    this.message = 'Loading...'
    this.dateService.getComplexList().subscribe(models => {
      this.result = JSON.parse(JSON.stringify(models));
      this.dates = models.map(x => x.date);
      this.types = models.map(x => typeof x.date);
      this.isDate = models.map(x => x.date instanceof Date);
      this.message = undefined;
    });
  }

  public loadModelEnumerable(): void {
    this.message = 'Loading...'
    this.dateService.getComplexEnumerable().subscribe(models => {
      this.result = JSON.parse(JSON.stringify(models));
      this.dates = models.map(x => x.date);
      this.types = models.map(x => typeof x.date);
      this.isDate = models.map(x => x.date instanceof Date);
      this.message = undefined;
    });
  }

  public loadWrappedArray(): void {
    this.message = 'Loading...'
    this.dateService.getWrappedArray().subscribe(wrapper => {
      this.result = JSON.parse(JSON.stringify(wrapper));
      this.dates = wrapper.dates;
      this.types = wrapper.dates.map(x => typeof x);
      this.isDate = wrapper.dates.map(x => x instanceof Date);
      this.message = undefined;
    });
  }

  public loadWrappedModel(): void {
    this.message = 'Loading...'
    this.dateService.getWrappedModel().subscribe(wrapper => {
      this.result = JSON.parse(JSON.stringify(wrapper));
      this.dates = [wrapper.model.date];
      this.types = [typeof wrapper.model.date];
      this.isDate = [wrapper.model.date instanceof Date];
      this.message = undefined;
    });
  }

  public loadWrappedModelArray(): void {
    this.message = 'Loading...'
    this.dateService.getWrappedModelArray().subscribe(wrapper => {
      this.result = JSON.parse(JSON.stringify(wrapper));
      this.dates = wrapper.models.map(x => x.date);
      this.types = wrapper.models.map(x => typeof x.date);
      this.isDate = wrapper.models.map(x => x.date instanceof Date);
      this.message = undefined;
    });
  }

  public loadWrappedModelWithDate(): void {
    this.message = 'Loading...'
    this.dateService.getWrappedModelWithDate().subscribe(wrapper => {
      this.result = JSON.parse(JSON.stringify(wrapper));
      this.dates = [wrapper.date, wrapper.model.date];
      this.types = [typeof wrapper.date, typeof wrapper.model.date];
      this.isDate = [wrapper.date instanceof Date, wrapper.model.date instanceof Date];
      this.message = undefined;
    });
  }

  public send(): void {
    this.message = 'Sending...'
    this.dateService.post(new Date()).subscribe(() => this.message = 'Sent!');
  }
}
