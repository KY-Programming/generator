/* eslint-disable */
// tslint:disable

import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class HardcodedActionRoutesService {
    private readonly http: HttpClient;
    private serviceUrlValue: string = "";
    public httpOptions: {} = {};

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

    public getAll(httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/hardcodedactionroutes/list";
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getById(id: number, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/hardcodedactionroutes/by-id";
        url = this.append(url, id, undefined, "/");
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getMixedCase(httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/hardcodedactionroutes/Mixed/Case/Segment";
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public update(value: string, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/hardcodedactionroutes/save";
        url = this.append(url, value, "value");
        this.http.post<void>(url, undefined, httpOptions).subscribe(() => {
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

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
