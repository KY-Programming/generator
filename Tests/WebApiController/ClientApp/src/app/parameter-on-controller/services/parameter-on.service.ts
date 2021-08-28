/* eslint-disable */
// tslint:disable

import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class ParameterOnService {
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

    public get(test: string, id: string, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/parameteron";
        url = this.append(url, id, undefined, "/");
        url += "/get";
        url = this.append(url, test, "test");
        this.http.get<void>(url, httpOptions).subscribe(() => {
            subject.next();
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
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
