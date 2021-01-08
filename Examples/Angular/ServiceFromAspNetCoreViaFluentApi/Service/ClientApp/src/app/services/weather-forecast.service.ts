﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated with KY.Generator 6.2.2.0
//      Manual changes to this file may cause unexpected behavior in your application.
//      Manual changes to this file will be overwritten if the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
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
    public serviceUrl: string = "";

    public constructor(http: HttpClient) {
        this.http = http;
    }

    public get(httpOptions: {} = undefined): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        this.http.get<WeatherForecast[]>(this.serviceUrl + "/weatherforecast", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }
}

// outputid:9a19103f-16f7-4668-be54-9a1e7a4f7556
