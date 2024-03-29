﻿/* eslint-disable */
// tslint:disable

import { DateModel } from "../models/date-model";
import { ExclusiveGenericComplexResult } from "../models/exclusive-generic-complex-result";
import { GenericResult } from "../models/generic-result";
import { SelfReferencingModel } from "../models/self-referencing-model";
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

    public get(subject: string, httpOptions?: {}): Observable<void> {
        let rxjsSubject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/get";
        url = this.append(url, subject, "subject");
        this.http.get<void>(url, httpOptions).subscribe(() => {
            rxjsSubject.next();
            rxjsSubject.complete();
        }, (error) => rxjsSubject.error(error));
        return rxjsSubject;
    }

    public post(subject: string, httpOptions?: {}): Observable<void> {
        let rxjsSubject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/post";
        url = this.append(url, subject, "subject");
        this.http.post<void>(url, undefined, httpOptions).subscribe(() => {
            rxjsSubject.next();
            rxjsSubject.complete();
        }, (error) => rxjsSubject.error(error));
        return rxjsSubject;
    }

    public cancelable(subject: string, httpOptions?: {}): Observable<string[]> {
        let rxjsSubject = new Subject<string[]>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/cancelable";
        url = this.append(url, subject, "subject");
        this.http.get<string[]>(url, httpOptions).subscribe((result) => {
            rxjsSubject.next(this.fixUndefined(result));
            rxjsSubject.complete();
        }, (error) => rxjsSubject.error(error));
        return rxjsSubject;
    }

    public string(httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/string";
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getGuid(httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/getguid";
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result?.replace(/(^"|"$)/g, "")));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public withDI(value: number, httpOptions?: {}): Observable<boolean> {
        let subject = new Subject<boolean>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/withdi";
        url = this.append(url, value, "value");
        this.http.get<boolean>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public fromHeader(value?: number, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/fromheader";
        url = this.append(url, value, "value");
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public fromQuery(queryValue?: string, value?: number, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/fromquery";
        url = this.append(url, queryValue, "queryValue");
        url = this.append(url, value, "value");
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public fromQueryArray(queryArray: string[], httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/fromqueryarray";
        queryArray.forEach((entry) => url = this.append(url, entry, "queryArray"));
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public genericResult(value1: string, value2: string, httpOptions?: {}): Observable<GenericResult<string>> {
        let subject = new Subject<GenericResult<string>>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/genericresult";
        url = this.append(url, value1, "value1");
        url = this.append(url, value2, "value2");
        this.http.get<GenericResult<string>>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public genericComplexResult(httpOptions?: {}): Observable<GenericResult<ExclusiveGenericComplexResult>> {
        let subject = new Subject<GenericResult<ExclusiveGenericComplexResult>>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/genericcomplexresult";
        this.http.get<GenericResult<ExclusiveGenericComplexResult>>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getGenericWithModel(httpOptions?: {}): Observable<GenericResult<DateModel>> {
        let subject = new Subject<GenericResult<DateModel>>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/getgenericwithmodel";
        this.http.get<GenericResult<DateModel>>(url, httpOptions).subscribe((result) => {
            this.convertGenericResultDate(result);
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWithOptional(required: number, optional?: string, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/getwithoptional";
        url = this.append(url, required, "required");
        url = this.append(url, optional, "optional");
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getInlineWithOptional(required: number, optional?: string, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/getinlinewithoptional";
        url = this.append(url, required, undefined, "/");
        url = this.append(url, optional, undefined, "/");
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getNamedInlineWithOptional(required: number, optional?: string, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/getnamedinlinewithoptional/required";
        url = this.append(url, required, undefined, "/");
        url += "/optional";
        url = this.append(url, optional, undefined, "/");
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWithAbsoluteRoute(httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/test/edgecases/getwithabsoluteroute";
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWithAbsoluteRouteAndParameter(id: number, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/test";
        url = this.append(url, id, undefined, "/");
        url += "/getwithabsoluterouteandparameter/edgecases";
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public unknownResult<TDefault = unknown>(value: string, httpOptions?: {}): Observable<TDefault> {
        let subject = new Subject<TDefault>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/unknownresult";
        url = this.append(url, value, "value");
        this.http.get<TDefault>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public selfReferencing(httpOptions?: {}): Observable<SelfReferencingModel> {
        let subject = new Subject<SelfReferencingModel>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/edgecases/selfreferencing";
        this.http.get<SelfReferencingModel>(url, httpOptions).subscribe((result) => {
            this.convertSelfReferencingModelDate(result);
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

    private convertDate(value: string | Date | undefined): Date | undefined {
        return value === "0001-01-01T00:00:00" ? new Date("0001-01-01T00:00:00Z") : typeof(value) === "string" ? new Date(value) : value;
    }

    public convertGenericResultDate(model: GenericResult<DateModel>): void {
        if (!model) {
            return;
        }
        model.rows.forEach((m) => this.convertDateModelDate(m))
    }

    public convertDateModelDate(model: DateModel): void {
        if (!model) {
            return;
        }
        model.date = this.convertDate(model.date);
    }

    public convertSelfReferencingModelDate(model: SelfReferencingModel): void {
        if (!model) {
            return;
        }
        model.children.forEach((m) => this.convertSelfReferencingModelDate(m))
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
