/* eslint-disable */
// tslint:disable

import { CasingModel } from "../models/casing-model";
import { CasingWithMappingModel } from "../models/casing-with-mapping-model";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class FixCasingService {
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
        this.serviceUrl = document.baseURI ?? "";
    }

    public get(httpOptions?: {}): Observable<CasingModel> {
        let subject = new Subject<CasingModel>();
        let url: string = this.serviceUrl + "/fixcasing/get";
        this.http.get<CasingModel>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public post(model: CasingModel, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/fixcasing/post";
        this.http.post<void>(url, model, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWithMapping(httpOptions?: {}): Observable<CasingWithMappingModel> {
        let subject = new Subject<CasingWithMappingModel>();
        let url: string = this.serviceUrl + "/fixcasing/getwithmapping";
        this.http.get<CasingWithMappingModel>(url, httpOptions).subscribe((result) => {
            result.allupper = result.allupper || result["ALLUPPER"];
            delete result['ALLUPPER'];
            result.pascalCase = result.pascalCase || result["PascalCase"];
            delete result['PascalCase'];
            result.snakeCase = result.snakeCase || result["snake_case"];
            delete result['snake_case'];
            result.upperSnakeCase = result.upperSnakeCase || result["UPPER_SNAKE_CASE"];
            delete result['UPPER_SNAKE_CASE'];
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getArrayWithMapping(httpOptions?: {}): Observable<CasingWithMappingModel[]> {
        let subject = new Subject<CasingWithMappingModel[]>();
        let url: string = this.serviceUrl + "/fixcasing/getarraywithmapping";
        this.http.get<CasingWithMappingModel[]>(url, httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => {
                    entry.allupper = entry.allupper || entry["ALLUPPER"];
                    delete entry['ALLUPPER'];
                    entry.pascalCase = entry.pascalCase || entry["PascalCase"];
                    delete entry['PascalCase'];
                    entry.snakeCase = entry.snakeCase || entry["snake_case"];
                    delete entry['snake_case'];
                    entry.upperSnakeCase = entry.upperSnakeCase || entry["UPPER_SNAKE_CASE"];
                    delete entry['UPPER_SNAKE_CASE'];
                })
            }
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public postWithMapping(model: CasingWithMappingModel, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        let url: string = this.serviceUrl + "/fixcasing/postwithmapping";
        this.http.post<void>(url, model, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public fixUndefined(value: any): any {
        if (! value) {
            return value ??  undefined;
        }
        if (Array.isArray(value)) {
            value.forEach((entry, index) => value[index] = this.fixUndefined(entry));
        }
        if (typeof value === 'object') {
            for (const key of Object.keys(value)) { value[key] = this.fixUndefined(value[key]); }
        }
        return value;
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
