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
        let url: string = this.serviceUrl + "/routed";
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

    public getThree(httpOptions?: {}): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        let url: string = this.serviceUrl + "/routed/three";
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

    public getThreeDays(httpOptions?: {}): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        let url: string = this.serviceUrl + "/routed/three/days";
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

    public getNext(days: number, httpOptions?: {}): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        let url: string = this.serviceUrl + "/routed/next";
        url = this.append(url, days, undefined, "/");
        url += "/days";
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

    public test(test: number, days: number, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/routed/next";
        url = this.append(url, days, undefined, "/");
        url + "/days";
        url = this.append(url, test, undefined, "/");
        this.http.get<void>(url, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test2(test: number, days: number, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/routed/test2";
        url = this.append(url, test, "test");
        url = this.append(url, days, "days");
        this.http.get<void>(url, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test3(test: number, days: number, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/routed/test3";
        url = this.append(url, test, undefined, "/");
        url = this.append(url, days, "days");
        this.http.get<void>(url, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test4(days: number, test: number, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/routed/test4";
        url = this.append(url, test, undefined, "/");
        url = this.append(url, days, "days");
        this.http.get<void>(url, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getTest5(test: number, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/routed/test5";
        url = this.append(url, test, undefined, "/");
        this.http.get<void>(url, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public postTest5(test: number, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/routed/test5";
        url = this.append(url, test, undefined, "/");
        this.http.post<void>(url, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test6(test: number, httpOptions?: {}): Observable<string[]> {
        let subject = new Subject<string[]>();
        let url: string = this.serviceUrl + "/routed/test6";
        url = this.append(url, test, undefined, "/");
        this.http.get<string[]>(url, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test7(test: number, httpOptions?: {}): Observable<string[]> {
        let subject = new Subject<string[]>();
        let url: string = this.serviceUrl + "/routed/test7";
        url = this.append(url, test, undefined, "/");
        this.http.get<string[]>(url, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test8(httpOptions?: {}): Observable<string[]> {
        let subject = new Subject<string[]>();
        let url: string = this.serviceUrl + "/routed/test8";
        this.http.get<string[]>(url, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test9(httpOptions?: {}): Observable<string[]> {
        let subject = new Subject<string[]>();
        let url: string = this.serviceUrl + "/routed/test9";
        this.http.get<string[]>(url, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test10(httpOptions?: {}): Observable<string[]> {
        let subject = new Subject<string[]>();
        let url: string = this.serviceUrl + "/routed/test10";
        this.http.get<string[]>(url, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test11(httpOptions?: {}): Observable<string[]> {
        let subject = new Subject<string[]>();
        let url: string = this.serviceUrl + "/routed/test11";
        this.http.get<string[]>(url, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test12(httpOptions?: {}): Observable<string[]> {
        let subject = new Subject<string[]>();
        let url: string = this.serviceUrl + "/routed/test12";
        this.http.get<string[]>(url, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public test13(httpOptions?: {}): Observable<string[]> {
        let subject = new Subject<string[]>();
        let url: string = this.serviceUrl + "/routed/test13";
        this.http.get<string[]>(url, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public append(url: string, value: {toString(): string} | undefined | null, parameterName: string = "", separator: string = ""): string {
        if (! parameterName) {
            return url + separator + (value === null || value === undefined ? "" : value.toString());
        }
        if (value !== null && value !== undefined) {
            return url + (url.indexOf("?") === -1 ? "?" : "&") + parameterName + "=" + value.toString();
        }
        return url;
    }

    public convertToDate(value: string | Date): Date {
        return typeof(value) === "string" ? new Date(value) : value;
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
