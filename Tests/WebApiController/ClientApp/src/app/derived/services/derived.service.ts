/* eslint-disable */
// tslint:disable

import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class DerivedService {
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

    public get(httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/derived/get", httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public overwritten(httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/derived/overwritten", httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getBase(httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/derived/getbase", httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public notOverwritten(httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/derived/notoverwritten", httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
