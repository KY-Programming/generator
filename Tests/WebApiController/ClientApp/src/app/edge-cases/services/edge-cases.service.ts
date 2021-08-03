/* eslint-disable */
// tslint:disable

import { GenericResult } from "../models/generic-result";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class EdgeCasesService {
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

    public get(subject: string, httpOptions?: {}): Observable<void> {
        let rxjsSubject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/edgecases/get" + "?subject=" + this.convertAny(subject), httpOptions).subscribe(() => {
            rxjsSubject.next();
            rxjsSubject.complete();
        }, (error) => rxjsSubject.error(error));
        return rxjsSubject;
    }

    public post(subject: string, httpOptions?: {}): Observable<void> {
        let rxjsSubject = new Subject<void>();
        this.http.post<void>(this.serviceUrl + "/edgecases/post" + "?subject=" + this.convertAny(subject), httpOptions).subscribe(() => {
            rxjsSubject.next();
            rxjsSubject.complete();
        }, (error) => rxjsSubject.error(error));
        return rxjsSubject;
    }

    public cancelable(subject: string, httpOptions?: {}): Observable<string[]> {
        let rxjsSubject = new Subject<string[]>();
        this.http.get<string[]>(this.serviceUrl + "/edgecases/cancelable" + "?subject=" + this.convertAny(subject), httpOptions).subscribe((result) => {
            rxjsSubject.next(result);
            rxjsSubject.complete();
        }, (error) => rxjsSubject.error(error));
        return rxjsSubject;
    }

    public string(httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...httpOptions};
        this.http.get<string>(this.serviceUrl + "/edgecases/string", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getGuid(httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        this.http.get<string>(this.serviceUrl + "/edgecases/getguid", httpOptions).subscribe((result) => {
            subject.next(result.replace(/(^"|"$)/g, ""));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public withDI(value: number, httpOptions?: {}): Observable<boolean> {
        let subject = new Subject<boolean>();
        this.http.get<boolean>(this.serviceUrl + "/edgecases/withdi" + "?value=" + this.convertAny(value), httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public fromHeader(value?: number, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...httpOptions};
        this.http.get<string>(this.serviceUrl + "/edgecases/fromheader" + "?value=" + this.convertAny(value), httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public fromQuery(queryValue?: string, value?: number, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...httpOptions};
        this.http.get<string>(this.serviceUrl + "/edgecases/fromquery" + "?queryValue=" + this.convertAny(queryValue) + "&value=" + this.convertAny(value), httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public fromQueryArray(queryArray: string[], httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        let queryArrayJoin: string = queryArray.map((x, index) => index ? "queryArray=" + this.convertAny(x) : this.convertAny(x)).join("&");
        httpOptions = { responseType: 'text', ...httpOptions};
        this.http.get<string>(this.serviceUrl + "/edgecases/fromqueryarray" + "?queryArray=" + this.convertAny(queryArrayJoin), httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public genericResult(value1: string, value2: string, httpOptions?: {}): Observable<GenericResult<string>> {
        let subject = new Subject<GenericResult<string>>();
        this.http.get<GenericResult<string>>(this.serviceUrl + "/edgecases/genericresult" + "?value1=" + this.convertAny(value1) + "&value2=" + this.convertAny(value2), httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getGenericWithModel(httpOptions?: {}): Observable<GenericResult<DateModel>> {
        let subject = new Subject<GenericResult<DateModel>>();
        this.http.get<GenericResult<DateModel>>(this.serviceUrl + "/edgecases/getgenericwithmodel", httpOptions).subscribe((result) => {
            if (result) {
                if (result.rows) {
                    result.rows.forEach((entry) => {
                        entry.date = this.convertToDate(entry.date);
                    });
                }
            }
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
