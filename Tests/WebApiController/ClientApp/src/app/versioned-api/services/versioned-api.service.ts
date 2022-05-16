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
export class VersionedApiService {
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
        this.serviceUrl = document.baseURI ?? "";
    }

    public get(httpOptions?: {}): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        let url: string = this.serviceUrl + "/versionedapi";
        this.http.get<WeatherForecast[]>(url, httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => {
                    entry.date = this.convertToDate(entry.date);
                });
            }
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getNext(days: number, httpOptions?: {}): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        let url: string = this.serviceUrl + "/versionedapi/next";
        url = this.append(url, days, undefined, "/");
        url += "/days";
        this.http.get<WeatherForecast[]>(url, httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => {
                    entry.date = this.convertToDate(entry.date);
                });
            }
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getNext2(days: number, httpOptions?: {}): Observable<WeatherForecast[]> {
        let subject = new Subject<WeatherForecast[]>();
        let url: string = this.serviceUrl + "/versionedapi/next-days";
        url = this.append(url, days, "days");
        this.http.get<WeatherForecast[]>(url, httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => {
                    entry.date = this.convertToDate(entry.date);
                });
            }
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWithAbsoluteRoute(httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...httpOptions};
        let url: string = this.serviceUrl + "/api/test/versionedapi/getwithabsoluteroute";
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
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

    public convertToDate(value: string | Date | undefined): Date | undefined {
        return value === "0001-01-01T00:00:00" ? new Date("0001-01-01T00:00:00Z") : typeof(value) === "string" ? new Date(value) : value;
    }

    public fixUndefined(value: any): any {
        if (! value) {
            return value ??  undefined;
        }
        if (Array.isArray(value)) {
            value.forEach((entry, index) => value[index] = this.fixUndefined(entry));
        }
        if (typeof value === 'object') {
            for (const key of Object.keys(value)) { value[key] = this.fixUndefined(value[key]); }
        }
        return value;
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
