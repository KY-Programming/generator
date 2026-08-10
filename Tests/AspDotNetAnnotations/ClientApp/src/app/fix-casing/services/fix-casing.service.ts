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
    public httpOptions: {} = {};

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
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/fixcasing/get";
        this.http.get<CasingModel>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public post(model: CasingModel, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/fixcasing/post";
        this.http.post<void>(url, model, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWithMapping(httpOptions?: {}): Observable<CasingWithMappingModel> {
        let subject = new Subject<CasingWithMappingModel>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/fixcasing/getwithmapping";
        this.http.get<CasingWithMappingModel>(url, httpOptions).subscribe((result) => {
            let mapped: Record<string, any> = result;
            mapped["allupper"] = mapped["allupper"] || mapped["ALLUPPER"];
            delete mapped['ALLUPPER'];
            mapped["pascalCase"] = mapped["pascalCase"] || mapped["PascalCase"];
            delete mapped['PascalCase'];
            mapped["snakeCase"] = mapped["snakeCase"] || mapped["snake_case"];
            delete mapped['snake_case'];
            mapped["upperSnakeCase"] = mapped["upperSnakeCase"] || mapped["UPPER_SNAKE_CASE"];
            delete mapped['UPPER_SNAKE_CASE'];
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getArrayWithMapping(httpOptions?: {}): Observable<CasingWithMappingModel[]> {
        let subject = new Subject<CasingWithMappingModel[]>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/fixcasing/getarraywithmapping";
        this.http.get<CasingWithMappingModel[]>(url, httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => {
                    let mapped: Record<string, any> = entry;
                    mapped["allupper"] = mapped["allupper"] || mapped["ALLUPPER"];
                    delete mapped['ALLUPPER'];
                    mapped["pascalCase"] = mapped["pascalCase"] || mapped["PascalCase"];
                    delete mapped['PascalCase'];
                    mapped["snakeCase"] = mapped["snakeCase"] || mapped["snake_case"];
                    delete mapped['snake_case'];
                    mapped["upperSnakeCase"] = mapped["upperSnakeCase"] || mapped["UPPER_SNAKE_CASE"];
                    delete mapped['UPPER_SNAKE_CASE'];
                })
            }
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public postWithMapping(model: CasingWithMappingModel, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/fixcasing/postwithmapping";
        this.http.post<void>(url, model, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    private fixUndefined(value: any): any {
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
