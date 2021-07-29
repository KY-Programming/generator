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
        this.http.get<RenameModel>(this.serviceUrl + "/rename" + "?id=" + this.convertAny(id), httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public removeDummy(id: number, httpOptions?: {}): Observable<Data> {
        let subject = new Subject<Data>();
        this.http.get<Data>(this.serviceUrl + "/rename" + "?id=" + this.convertAny(id), httpOptions).subscribe((result) => {
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
