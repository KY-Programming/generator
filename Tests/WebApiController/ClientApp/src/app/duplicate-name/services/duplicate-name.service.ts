/* eslint-disable */
// tslint:disable

import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class DuplicateNameService {
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

    public testA(id: number, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/duplicatename";
        url = this.append(url, id, undefined, "/");
        this.http.get<void>(url, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public testAById(id: number, variantA: string, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/duplicatename";
        url = this.append(url, id, undefined, "/");
        url = this.append(url, variantA, undefined, "/");
        this.http.get<void>(url, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public testAByIdAndVariantA(id: number, variantA: string, variantB: string, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/duplicatename";
        url = this.append(url, id, undefined, "/");
        url = this.append(url, variantA, undefined, "/");
        url = this.append(url, variantB, undefined, "/");
        this.http.get<void>(url, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public testB(id: number, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...httpOptions};
        let url: string = this.serviceUrl + "/duplicatename/testb";
        url = this.append(url, id, "id");
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public testBById(id: string, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...httpOptions};
        let url: string = this.serviceUrl + "/duplicatename";
        url = this.append(url, id, "id");
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
