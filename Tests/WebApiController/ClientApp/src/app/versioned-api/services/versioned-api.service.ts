// tslint:disable

import { WeatherForecast } from "../models/weather-forecast";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class VersionedApiService {
    private readonly http: HttpClient;
    public serviceUrl: string = "";

    public constructor(http: HttpClient) {
        this.http = http;
    }

    public get(httpOptions: {} = undefined): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        this.http.get<WeatherForecast[]>(this.serviceUrl + "/versionedapi" + "?api-version=1.0", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getNext(days: number, httpOptions: {} = undefined): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        this.http.get<WeatherForecast[]>(this.serviceUrl + "/versionedapi/next/" + days + "/days" + "?api-version=1.0", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getNext2(days: number, httpOptions: {} = undefined): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        this.http.get<WeatherForecast[]>(this.serviceUrl + "/versionedapi/next-days" + "?api-version=1.0" + "&days=" + this.convertAny(days), httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public convertAny(value: any): string {
        return value === null || value === undefined ? "" : value.toString();
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
