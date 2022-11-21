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

    public get serviceUrl(): string {
        return this.serviceUrlValue;
    }
    public set serviceUrl(value: string) {
        this.serviceUrlValue = value.replace(/\/+$/, "");
    }

    public constructor(http: HttpClient) {
        this.http = http;
    }

    public get(httpOptions?: {}): Observable<Date> {
        let subject = new Subject<Date>();
        let url: string = this.serviceUrl + "/api/date/get";
        this.http.get<Date>(url, httpOptions).subscribe((result) => {
            subject.next(this.convertToDate(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getArray(httpOptions?: {}): Observable<Date[]> {
        let subject = new Subject<Date[]>();
        let url: string = this.serviceUrl + "/api/date/getarray";
        this.http.get<Date[]>(url, httpOptions).subscribe((result) => {
            subject.next(result.map((entry) => this.convertToDate(entry)));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getList(httpOptions?: {}): Observable<Date[]> {
        let subject = new Subject<Date[]>();
        let url: string = this.serviceUrl + "/api/date/getlist";
        this.http.get<Date[]>(url, httpOptions).subscribe((result) => {
            subject.next(result.map((entry) => this.convertToDate(entry)));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getEnumerable(httpOptions?: {}): Observable<Date[]> {
        let subject = new Subject<Date[]>();
        let url: string = this.serviceUrl + "/api/date/getenumerable";
        this.http.get<Date[]>(url, httpOptions).subscribe((result) => {
            subject.next(result.map((entry) => this.convertToDate(entry)));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getComplex(httpOptions?: {}): Observable<DateModel> {
        let subject = new Subject<DateModel>();
        let url: string = this.serviceUrl + "/api/date/getcomplex";
        this.http.get<DateModel>(url, httpOptions).subscribe((result) => {
            if (result) {
                result.date = this.convertToDate(result.date);
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getComplexArray(httpOptions?: {}): Observable<DateModel[]> {
        let subject = new Subject<DateModel[]>();
        let url: string = this.serviceUrl + "/api/date/getcomplexarray";
        this.http.get<DateModel[]>(url, httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => {
                    entry.date = this.convertToDate(entry.date);
                });
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getComplexList(httpOptions?: {}): Observable<DateModel[]> {
        let subject = new Subject<DateModel[]>();
        let url: string = this.serviceUrl + "/api/date/getcomplexlist";
        this.http.get<DateModel[]>(url, httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => {
                    entry.date = this.convertToDate(entry.date);
                });
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getComplexEnumerable(httpOptions?: {}): Observable<DateModel[]> {
        let subject = new Subject<DateModel[]>();
        let url: string = this.serviceUrl + "/api/date/getcomplexenumerable";
        this.http.get<DateModel[]>(url, httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => {
                    entry.date = this.convertToDate(entry.date);
                });
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedArray(httpOptions?: {}): Observable<DateArrayWrapper> {
        let subject = new Subject<DateArrayWrapper>();
        let url: string = this.serviceUrl + "/api/date/getwrappedarray";
        this.http.get<DateArrayWrapper>(url, httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedModel(httpOptions?: {}): Observable<DateModelWrapper> {
        let subject = new Subject<DateModelWrapper>();
        let url: string = this.serviceUrl + "/api/date/getwrappedmodel";
        this.http.get<DateModelWrapper>(url, httpOptions).subscribe((result) => {
            if (result) {
                if (result.model) {
                    result.model.date = this.convertToDate(result.model.date);
                }
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedModelList(httpOptions?: {}): Observable<DateModelWrapper[]> {
        let subject = new Subject<DateModelWrapper[]>();
        let url: string = this.serviceUrl + "/api/date/getwrappedmodellist";
        this.http.get<DateModelWrapper[]>(url, httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => {
                    if (entry.model) {
                        entry.model.date = this.convertToDate(entry.model.date);
                    }
                });
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedModelListWrapper(httpOptions?: {}): Observable<DateModelWrapperListWrapper> {
        let subject = new Subject<DateModelWrapperListWrapper>();
        let url: string = this.serviceUrl + "/api/date/getwrappedmodellistwrapper";
        this.http.get<DateModelWrapperListWrapper>(url, httpOptions).subscribe((result) => {
            if (result) {
                if (result.list) {
                    result.list.forEach((entry) => {
                        if (entry.model) {
                            entry.model.date = this.convertToDate(entry.model.date);
                        }
                    });
                }
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedModelListWrapperList(httpOptions?: {}): Observable<DateModelWrapperListWrapper[]> {
        let subject = new Subject<DateModelWrapperListWrapper[]>();
        let url: string = this.serviceUrl + "/api/date/getwrappedmodellistwrapperlist";
        this.http.get<DateModelWrapperListWrapper[]>(url, httpOptions).subscribe((result) => {
            if (result) {
                result.forEach((entry) => {
                    if (entry.list) {
                        entry.list.forEach((entry) => {
                            if (entry.model) {
                                entry.model.date = this.convertToDate(entry.model.date);
                            }
                        });
                    }
                });
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedModelWithDate(httpOptions?: {}): Observable<DateModelWrapperWithDate> {
        let subject = new Subject<DateModelWrapperWithDate>();
        let url: string = this.serviceUrl + "/api/date/getwrappedmodelwithdate";
        this.http.get<DateModelWrapperWithDate>(url, httpOptions).subscribe((result) => {
            if (result) {
                result.date = this.convertToDate(result.date);
                if (result.model) {
                    result.model.date = this.convertToDate(result.model.date);
                }
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedModelArray(httpOptions?: {}): Observable<DateModelArrayWrapper> {
        let subject = new Subject<DateModelArrayWrapper>();
        let url: string = this.serviceUrl + "/api/date/getwrappedmodelarray";
        this.http.get<DateModelArrayWrapper>(url, httpOptions).subscribe((result) => {
            if (result) {
                if (result.models) {
                    result.models.forEach((entry) => {
                        entry.date = this.convertToDate(entry.date);
                    });
                }
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getGenericWithModel(httpOptions?: {}): Observable<GenericResult<DateModel>> {
        let subject = new Subject<GenericResult<DateModel>>();
        let url: string = this.serviceUrl + "/api/date/getgenericwithmodel";
        this.http.get<GenericResult<DateModel>>(url, httpOptions).subscribe((result) => {
            if (result) {
                if (result.rows) {
                    result.rows.forEach((entry) => {
                        entry.date = this.convertToDate(entry.date);
                    });
                }
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public post(date: Date, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
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

    public appendDate(url: string, date: Date, parameterName: string = "", separator: string = ""): string {
        return this.append(url, date === null || date === undefined ? "" : typeof(date) === "string" ? date : date.toISOString(), parameterName, separator);
    }

    public convertToDate(value: string | Date): Date {
        return typeof(value) === "string" ? new Date(value) : value;
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
