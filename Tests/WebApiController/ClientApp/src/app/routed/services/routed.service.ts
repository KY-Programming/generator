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
export class RoutedService {
    private readonly http: HttpClient;
    public serviceUrl: string = "";

    public constructor(http: HttpClient) {
        this.http = http;
    }

    public get(httpOptions: {} = undefined): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        this.http.get<WeatherForecast[]>(this.serviceUrl + "/routed", httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => entry.date = this.convertToDate(entry.date));
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getThree(httpOptions: {} = undefined): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        this.http.get<WeatherForecast[]>(this.serviceUrl + "/routed/three", httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => entry.date = this.convertToDate(entry.date));
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getThreeDays(httpOptions: {} = undefined): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        this.http.get<WeatherForecast[]>(this.serviceUrl + "/routed/three/days", httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => entry.date = this.convertToDate(entry.date));
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getNext(days: number, httpOptions: {} = undefined): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        this.http.get<WeatherForecast[]>(this.serviceUrl + "/routed/next/" + days + "/days", httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => entry.date = this.convertToDate(entry.date));
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test(test: number, days: number, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/routed/next/" + days + "/days/" + test, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test2(test: number, days: number, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/routed/test2" + "?test=" + this.convertAny(test) + "&days=" + this.convertAny(days), httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test3(test: number, days: number, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/routed/test3/" + test + "?days=" + this.convertAny(days), httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test4(days: number, test: number, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/routed/test4/" + test + "?days=" + this.convertAny(days), httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getTest5(test: number, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/routed/test5/" + test, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public postTest5(test: number, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.post<void>(this.serviceUrl + "/routed/test5/" + test, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test6(test: number, httpOptions: {} = undefined): Observable<string[]> {
        let subject = new Subject<string[]>();
        this.http.get<string[]>(this.serviceUrl + "/routed/test6/" + test, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test7(test: number, httpOptions: {} = undefined): Observable<string[]> {
        let subject = new Subject<string[]>();
        this.http.get<string[]>(this.serviceUrl + "/routed/test7/" + test, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test8(httpOptions: {} = undefined): Observable<string[]> {
        let subject = new Subject<string[]>();
        this.http.get<string[]>(this.serviceUrl + "/routed/test8", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test9(httpOptions: {} = undefined): Observable<string[]> {
        let subject = new Subject<string[]>();
        this.http.get<string[]>(this.serviceUrl + "/routed/test9", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test10(httpOptions: {} = undefined): Observable<string[]> {
        let subject = new Subject<string[]>();
        this.http.get<string[]>(this.serviceUrl + "/routed/test10", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test11(httpOptions: {} = undefined): Observable<string[]> {
        let subject = new Subject<string[]>();
        this.http.get<string[]>(this.serviceUrl + "/routed/test11", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test12(httpOptions: {} = undefined): Observable<string[]> {
        let subject = new Subject<string[]>();
        this.http.get<string[]>(this.serviceUrl + "/routed/test12", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test13(httpOptions: {} = undefined): Observable<string[]> {
        let subject = new Subject<string[]>();
        this.http.get<string[]>(this.serviceUrl + "/routed/test13", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public convertAny(value: any): string {
        return value === null || value === undefined ? "" : value.toString();
    }

    public convertToDate(value: string | Date): Date {
        return typeof(value) === "string" ? new Date(value) : value;
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
