/* eslint-disable */
// tslint:disable

import { GetComplexModel } from "../models/get-complex-model";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class GetComplexService {
    private readonly http: HttpClient;
    public serviceUrl: string = "";

    public constructor(http: HttpClient) {
        this.http = http;
    }

    public get(httpOptions: {} = undefined): Observable<GetComplexModel> {
        let subject = new Subject<GetComplexModel>();
        this.http.get<GetComplexModel>(this.serviceUrl + "/getcomplex/get", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
