// tslint:disable

import { CasingModel } from "../models/casing-model";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class KeepCasingService {
    private readonly http: HttpClient;
    public serviceUrl: string = "";

    public constructor(http: HttpClient) {
        this.http = http;
    }

    public get(httpOptions: {} = undefined): Observable<CasingModel> {
        let subject = new Subject<CasingModel>();
        this.http.get<CasingModel>(this.serviceUrl + "/keepcasing", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public post(model: CasingModel, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.post<void>(this.serviceUrl + "/keepcasing" + "?model=" + this.convertAny(model), httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public convertAny(value: any): string {
        return value === null || value === undefined ? "" : value.toString();
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
