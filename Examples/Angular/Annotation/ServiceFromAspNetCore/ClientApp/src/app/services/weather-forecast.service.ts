﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated with KY.Generator 8.2.0.0
//
//      Manual changes to this file may cause unexpected behavior in your application.
//      Manual changes to this file will be overwritten if the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
/* eslint-disable */
// tslint:disable

import { WeatherForecast } from "../models/weather-forecast";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class WeatherForecastService {
    private readonly http: HttpClient;
    private serviceUrlValue: string = "";

    public get serviceUrl(): string {
        return this.serviceUrlValue;
    }
    public set serviceUrl(value: string) {
        this.serviceUrlValue = value.replace(/\/+$/, "");
    }

    public constructor(http: HttpClient) {
        this.http = http;
    }

    public get(httpOptions?: {}): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        let url: string = this.serviceUrl + "/weatherforecast";
        this.http.get<WeatherForecast[]>(url, httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => {
                    entry.date = this.convertToDate(entry.date);
                });
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public convertToDate(value: string | Date): Date {
        return typeof(value) === "string" ? new Date(value) : value;
    }
}

// outputid:c2cd74cf-53e6-4c11-86de-4f23690f7cf8
