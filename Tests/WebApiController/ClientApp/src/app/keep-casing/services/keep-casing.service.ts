/* eslint-disable */
// tslint:disable

import { KeepCasingModel } from "../models/keep-casing-model";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class KeepCasingService {
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

    public get(httpOptions?: {}): Observable<KeepCasingModel> {
        let subject = new Subject<KeepCasingModel>();
        let url: string = this.serviceUrl + "/keepcasing";
        this.http.get<KeepCasingModel>(url, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public post(model: KeepCasingModel, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/keepcasing";
        this.http.post<void>(url, model, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
