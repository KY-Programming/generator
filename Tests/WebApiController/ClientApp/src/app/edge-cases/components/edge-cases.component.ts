import { Component, OnInit } from '@angular/core';
import { EdgeCasesService } from '../services/edge-cases.service';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-edge-cases',
  templateUrl: './edge-cases.component.html',
  styleUrls: ['./edge-cases.component.css']
})
export class EdgeCasesComponent implements OnInit {
  public withDiValue: number;
  public guid: string;
  public headerValue: string;
  public headerResult: string;
  public unknownValue: string;
  public unknownResult: unknown;

  constructor(
    private readonly service: EdgeCasesService
  ) { }

  ngOnInit() {
    this.service.httpOptions = {
        headers: new HttpHeaders({'X-Test': 'Test'})
    }
  }

  public withDi(): void {
    this.service.withDI(this.withDiValue);
  }

  public getGuid(): void {
    this.guid = 'Loading...';
    this.service.getGuid().subscribe(guid => this.guid = guid, (error: Error) => this.guid = error.message);
  }

  public withHeader(): void {
    this.headerResult = 'Loading...';
    const value = Math.round(Math.random() * 10);
    const httpOptions = { headers: { 'headerValue': this.headerValue || '' } };
    this.service.fromHeader(value, httpOptions).subscribe(result => this.headerResult = result, error => this.headerResult = error.message);
  }

  public getUnknown(): void {
    this.unknownResult = 'Loading...';
    this.service.unknownResult(this.unknownValue).subscribe(result => this.unknownResult = (result as {value: string}).value);
  }
}
