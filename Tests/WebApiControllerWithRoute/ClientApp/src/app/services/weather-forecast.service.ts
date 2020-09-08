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
        this.http.get<WeatherForecast[]>(this.serviceUrl + "/WeatherForecast", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getThree(httpOptions: {} = undefined): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        this.http.get<WeatherForecast[]>(this.serviceUrl + "/WeatherForecast/three", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getThreeDays(httpOptions: {} = undefined): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        this.http.get<WeatherForecast[]>(this.serviceUrl + "/WeatherForecast/three/days", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getNext(days: number, httpOptions: {} = undefined): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        this.http.get<WeatherForecast[]>(this.serviceUrl + "/WeatherForecast/next/" + days + "/days", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test(test: number, days: number, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/WeatherForecast/next/" + days + "/days/" + test, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test2(test: number, days: number, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/WeatherForecast/test2?test=" + this.convertAny(test) + "&days=" + this.convertAny(days), httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test3(test: number, days: number, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/WeatherForecast/test3/" + test + "?days=" + this.convertAny(days), httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test4(days: number, test: number, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/WeatherForecast/test4/" + test + "?days=" + this.convertAny(days), httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test5(test: number, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/WeatherForecast/test5/" + test, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test52(test: number, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.post<void>(this.serviceUrl + "/WeatherForecast/test5/{test}", test, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test6(test: number, httpOptions: {} = undefined): Observable<string[]> {
        let subject = new Subject<string[]>();
        this.http.get<string[]>(this.serviceUrl + "/WeatherForecast/test6/" + test, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test7(test: number, httpOptions: {} = undefined): Observable<string[]> {
        let subject = new Subject<string[]>();
        this.http.get<string[]>(this.serviceUrl + "/WeatherForecast/test7/" + test, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public convertAny(value: any): string {
        return value === null || value === undefined ? "" : value.toString();
    }
}