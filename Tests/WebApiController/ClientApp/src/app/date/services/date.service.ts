/* eslint-disable */
// tslint:disable

import { DateArrayWrapper } from "../models/date-array-wrapper";
import { DateModel } from "../models/date-model";
import { DateModelArrayWrapper } from "../models/date-model-array-wrapper";
import { DateModelWrapper } from "../models/date-model-wrapper";
import { DateModelWrapperListWrapper } from "../models/date-model-wrapper-list-wrapper";
import { DateModelWrapperWithDate } from "../models/date-model-wrapper-with-date";
import { GenericResult } from "../models/generic-result";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class DateService {
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

    public get(httpOptions?: {}): Observable<Date> {
        let subject = new Subject<Date>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/get";
        this.http.get<Date>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(this.convertDate(result)));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getArray(httpOptions?: {}): Observable<Date[]> {
        let subject = new Subject<Date[]>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getarray";
        this.http.get<Date[]>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result.map((entry) => this.convertDate(entry))));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getList(httpOptions?: {}): Observable<Date[]> {
        let subject = new Subject<Date[]>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getlist";
        this.http.get<Date[]>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result.map((entry) => this.convertDate(entry))));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getEnumerable(httpOptions?: {}): Observable<Date[]> {
        let subject = new Subject<Date[]>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getenumerable";
        this.http.get<Date[]>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result.map((entry) => this.convertDate(entry))));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getComplex(httpOptions?: {}): Observable<DateModel> {
        let subject = new Subject<DateModel>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getcomplex";
        this.http.get<DateModel>(url, httpOptions).subscribe((result) => {
            this.convertDateModelDate(result);
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getComplexArray(httpOptions?: {}): Observable<DateModel[]> {
        let subject = new Subject<DateModel[]>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getcomplexarray";
        this.http.get<DateModel[]>(url, httpOptions).subscribe((result) => {
            result.forEach((m) => this.convertDateModelDate(m));
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getComplexList(httpOptions?: {}): Observable<DateModel[]> {
        let subject = new Subject<DateModel[]>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getcomplexlist";
        this.http.get<DateModel[]>(url, httpOptions).subscribe((result) => {
            result.forEach((m) => this.convertDateModelDate(m));
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getComplexEnumerable(httpOptions?: {}): Observable<DateModel[]> {
        let subject = new Subject<DateModel[]>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getcomplexenumerable";
        this.http.get<DateModel[]>(url, httpOptions).subscribe((result) => {
            result.forEach((m) => this.convertDateModelDate(m));
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedArray(httpOptions?: {}): Observable<DateArrayWrapper> {
        let subject = new Subject<DateArrayWrapper>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getwrappedarray";
        this.http.get<DateArrayWrapper>(url, httpOptions).subscribe((result) => {
            this.convertDateArrayWrapperDate(result);
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedModel(httpOptions?: {}): Observable<DateModelWrapper> {
        let subject = new Subject<DateModelWrapper>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getwrappedmodel";
        this.http.get<DateModelWrapper>(url, httpOptions).subscribe((result) => {
            this.convertDateModelWrapperDate(result);
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedModelList(httpOptions?: {}): Observable<DateModelWrapper[]> {
        let subject = new Subject<DateModelWrapper[]>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getwrappedmodellist";
        this.http.get<DateModelWrapper[]>(url, httpOptions).subscribe((result) => {
            result.forEach((m) => this.convertDateModelWrapperDate(m));
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedModelListWrapper(httpOptions?: {}): Observable<DateModelWrapperListWrapper> {
        let subject = new Subject<DateModelWrapperListWrapper>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getwrappedmodellistwrapper";
        this.http.get<DateModelWrapperListWrapper>(url, httpOptions).subscribe((result) => {
            this.convertDateModelWrapperListWrapperDate(result);
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedModelListWrapperList(httpOptions?: {}): Observable<DateModelWrapperListWrapper[]> {
        let subject = new Subject<DateModelWrapperListWrapper[]>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getwrappedmodellistwrapperlist";
        this.http.get<DateModelWrapperListWrapper[]>(url, httpOptions).subscribe((result) => {
            result.forEach((m) => this.convertDateModelWrapperListWrapperDate(m));
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedModelWithDate(httpOptions?: {}): Observable<DateModelWrapperWithDate> {
        let subject = new Subject<DateModelWrapperWithDate>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getwrappedmodelwithdate";
        this.http.get<DateModelWrapperWithDate>(url, httpOptions).subscribe((result) => {
            this.convertDateModelWrapperWithDateDate(result);
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedModelArray(httpOptions?: {}): Observable<DateModelArrayWrapper> {
        let subject = new Subject<DateModelArrayWrapper>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getwrappedmodelarray";
        this.http.get<DateModelArrayWrapper>(url, httpOptions).subscribe((result) => {
            this.convertDateModelArrayWrapperDate(result);
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getGenericWithModel(httpOptions?: {}): Observable<GenericResult<DateModel>> {
        let subject = new Subject<GenericResult<DateModel>>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/getgenericwithmodel";
        this.http.get<GenericResult<DateModel>>(url, httpOptions).subscribe((result) => {
            this.convertGenericResultDate(result);
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public post(date: Date, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/date/post";
        url = this.appendDate(url, date, "date");
        this.http.post<void>(url, undefined, httpOptions).subscribe(() => {
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

    private appendDate(url: string, date: Date, parameterName: string = "", separator: string = ""): string {
        return this.append(url, date === null || date === undefined ? "" : typeof(date) === "string" ? date : date.toISOString(), parameterName, separator);
    }

    private convertDate(value: string | Date | undefined): Date | undefined {
        return value === "0001-01-01T00:00:00" ? new Date("0001-01-01T00:00:00Z") : typeof(value) === "string" ? new Date(value) : value;
    }

    public convertDateModelDate(model: DateModel): void {
        if (!model) {
            return;
        }
        model.date = this.convertDate(model.date);
    }

    public convertDateArrayWrapperDate(model: DateArrayWrapper): void {
        if (!model) {
            return;
        }
        model.dates = model.dates.map((m) => this.convertDate(m))
    }

    public convertDateModelWrapperDate(model: DateModelWrapper): void {
        if (!model) {
            return;
        }
        this.convertDateModelDate(model.model)
    }

    public convertDateModelWrapperListWrapperDate(model: DateModelWrapperListWrapper): void {
        if (!model) {
            return;
        }
        model.list.forEach((m) => this.convertDateModelWrapperDate(m))
    }

    public convertDateModelWrapperWithDateDate(model: DateModelWrapperWithDate): void {
        if (!model) {
            return;
        }
        model.date = this.convertDate(model.date);
        this.convertDateModelDate(model.model)
    }

    public convertDateModelArrayWrapperDate(model: DateModelArrayWrapper): void {
        if (!model) {
            return;
        }
        model.models.forEach((m) => this.convertDateModelDate(m))
    }

    public convertGenericResultDate(model: GenericResult<DateModel>): void {
        if (!model) {
            return;
        }
        model.rows.forEach((m) => this.convertDateModelDate(m))
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
