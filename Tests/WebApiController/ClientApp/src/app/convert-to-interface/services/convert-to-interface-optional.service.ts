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
        let url: string = this.serviceUrl + "/converttointerfaceoptional/get";
        url = this.append(url, subject, "subject");
        this.http.get<ConvertMeOptional>(url, httpOptions).subscribe((result) => {
            if (result) {
                result.dateTimeProperty = this.convertToDate(result.dateTimeProperty);
            }
            rxjsSubject.next(result);
            rxjsSubject.complete();
        }, (error) => rxjsSubject.error(error));
        return rxjsSubject;
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

    public convertToDate(value: string | Date): Date {
        return typeof(value) === "string" ? new Date(value) : value;
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
