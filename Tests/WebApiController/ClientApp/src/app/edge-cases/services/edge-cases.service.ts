/* eslint-disable */
// tslint:disable

import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class EdgeCasesService {
    private readonly http: HttpClient;
    public serviceUrl: string = "";

    public constructor(http: HttpClient) {
        this.http = http;
    }

    public get(subject: string, httpOptions: {} = undefined): Observable<void> {
        let rxjsSubject = new Subject<void>();
        this.http.get<void>(this.serviceUrl + "/edgecases" + "?subject=" + this.convertAny(subject), httpOptions).subscribe(() => {
            rxjsSubject.next();
            rxjsSubject.complete();
        }, (error) => rxjsSubject.error(error));
        return rxjsSubject;
    }

    public post(subject: string, httpOptions: {} = undefined): Observable<void> {
        let rxjsSubject = new Subject<void>();
        this.http.post<void>(this.serviceUrl + "/edgecases" + "?subject=" + this.convertAny(subject), httpOptions).subscribe(() => {
            rxjsSubject.next();
            rxjsSubject.complete();
        }, (error) => rxjsSubject.error(error));
        return rxjsSubject;
    }

    public cancelable(subject: string, httpOptions: {} = undefined): Observable<string[]> {
        let rxjsSubject = new Subject<string[]>();
        this.http.get<string[]>(this.serviceUrl + "/edgecases" + "?subject=" + this.convertAny(subject), httpOptions).subscribe((result) => {
            rxjsSubject.next(result);
            rxjsSubject.complete();
        }, (error) => rxjsSubject.error(error));
        return rxjsSubject;
    }

    public convertAny(value: any): string {
        return value === null || value === undefined ? "" : value.toString();
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
