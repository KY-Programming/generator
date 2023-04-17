/* eslint-disable */
// tslint:disable

import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class HttpTypesService {
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
    }

    public get(httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/httptypes";
        this.http.get<void>(url, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public post(httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/httptypes";
        this.http.post<void>(url, undefined, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public patch(test: string, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/httptypes";
        this.http.patch<void>(url, test, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public put(test: string, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/httptypes";
        this.http.put<void>(url, test, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public delete(httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/httptypes";
        this.http.delete<void>(url, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
