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
export class WarningService {
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

    public getWithBody(model: WeatherForecast, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/api/warning";
        url = this.append(url, model, "model");
        this.http.get<void>(url, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public postWithBody(model: WeatherForecast, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/api/warning";
        this.http.post<void>(url, model, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public patchWithBody(model: WeatherForecast, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/api/warning";
        this.http.patch<void>(url, model, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public putWithBody(model: WeatherForecast, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/api/warning";
        this.http.put<void>(url, model, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public deleteWithBody(model: WeatherForecast, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/api/warning";
        url = this.append(url, model, "model");
        this.http.delete<void>(url, httpOptions).subscribe(() => {
            subject.next();
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

    private fixUndefined(value: any): any {
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
