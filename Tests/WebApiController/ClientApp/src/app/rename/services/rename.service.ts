/* eslint-disable */
// tslint:disable

import { Data } from "../models/data";
import { RenameModel } from "../models/rename-model";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class RenameService {
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

    public renameDtoToModel(id: number, httpOptions?: {}): Observable<RenameModel> {
        let subject = new Subject<RenameModel>();
        let url: string = this.serviceUrl + "/rename";
        url = this.append(url, id, "id");
        this.http.get<RenameModel>(url, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public removeDummy(id: number, httpOptions?: {}): Observable<Data> {
        let subject = new Subject<Data>();
        let url: string = this.serviceUrl + "/rename";
        url = this.append(url, id, "id");
        this.http.get<Data>(url, httpOptions).subscribe((result) => {
            subject.next(result);
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
