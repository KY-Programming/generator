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
    }

    public testA(id: number, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/duplicatename/" + id, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public testAById(id: number, variantA: string, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/duplicatename/" + id + "/" + variantA, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public testAByIdAndVariantA(id: number, variantA: string, variantB: string, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/duplicatename/" + id + "/" + variantA + "/" + variantB, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public testB(id: number, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...httpOptions};
        this.http.get<string>(this.serviceUrl + "/duplicatename/testb" + "?id=" + this.convertAny(id), httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public testBById(id: string, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...httpOptions};
        this.http.get<string>(this.serviceUrl + "/duplicatename" + "?id=" + this.convertAny(id), httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public convertAny(value: any): string {
        return value === null || value === undefined ? "" : value.toString();
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
