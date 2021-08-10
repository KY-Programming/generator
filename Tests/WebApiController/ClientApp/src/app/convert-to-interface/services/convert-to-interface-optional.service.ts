/* eslint-disable */
// tslint:disable

import { ConvertMeOptional } from "../models/convert-me-optional";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class ConvertToInterfaceOptionalService {
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

    public get(subject: string, httpOptions?: {}): Observable<ConvertMeOptional> {
        let rxjsSubject = new Subject<ConvertMeOptional>();
        this.http.get<ConvertMeOptional>(this.serviceUrl + "/converttointerfaceoptional/get" + "?subject=" + this.convertAny(subject), httpOptions).subscribe((result) => {
            if (result) {
                result.dateTimeProperty = this.convertToDate(result.dateTimeProperty);
            }
            rxjsSubject.next(result);
            rxjsSubject.complete();
        }, (error) => rxjsSubject.error(error));
        return rxjsSubject;
    }

    public convertAny(value: any): string {
        return value === null || value === undefined ? "" : value.toString();
    }

    public convertToDate(value: string | Date): Date {
        return typeof(value) === "string" ? new Date(value) : value;
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
