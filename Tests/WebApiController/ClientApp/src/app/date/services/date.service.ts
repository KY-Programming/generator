/* eslint-disable */
// tslint:disable

import { DateArrayWrapper } from "../models/date-array-wrapper";
import { DateModel } from "../models/date-model";
import { DateModelArrayWrapper } from "../models/date-model-array-wrapper";
import { DateModelWrapper } from "../models/date-model-wrapper";
import { DateModelWrapperListWrapper } from "../models/date-model-wrapper-list-wrapper";
import { DateModelWrapperWithDate } from "../models/date-model-wrapper-with-date";
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

    public get(httpOptions: {} = undefined): Observable<Date> {
        let subject = new Subject<Date>();
        this.http.get<Date>(this.serviceUrl + "/api/date/get", httpOptions).subscribe((result) => {
            subject.next(this.convertToDate(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getArray(httpOptions: {} = undefined): Observable<Date[]> {
        let subject = new Subject<Date[]>();
        this.http.get<Date[]>(this.serviceUrl + "/api/date/getarray", httpOptions).subscribe((result) => {
            subject.next(result.map((entry) => this.convertToDate(entry)));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getList(httpOptions: {} = undefined): Observable<Date[]> {
        let subject = new Subject<Date[]>();
        this.http.get<Date[]>(this.serviceUrl + "/api/date/getlist", httpOptions).subscribe((result) => {
            subject.next(result.map((entry) => this.convertToDate(entry)));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getEnumerable(httpOptions: {} = undefined): Observable<Date[]> {
        let subject = new Subject<Date[]>();
        this.http.get<Date[]>(this.serviceUrl + "/api/date/getenumerable", httpOptions).subscribe((result) => {
            subject.next(result.map((entry) => this.convertToDate(entry)));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getComplex(httpOptions: {} = undefined): Observable<DateModel> {
        let subject = new Subject<DateModel>();
        this.http.get<DateModel>(this.serviceUrl + "/api/date/getcomplex", httpOptions).subscribe((result) => {
            if (result) {
                result.date = this.convertToDate(result.date);
            }
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getComplexArray(httpOptions: {} = undefined): Observable<DateModel[]> {
        let subject = new Subject<DateModel[]>();
        this.http.get<DateModel[]>(this.serviceUrl + "/api/date/getcomplexarray", httpOptions).subscribe((result) => {
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

    public getComplexList(httpOptions: {} = undefined): Observable<DateModel[]> {
        let subject = new Subject<DateModel[]>();
        this.http.get<DateModel[]>(this.serviceUrl + "/api/date/getcomplexlist", httpOptions).subscribe((result) => {
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

    public getComplexEnumerable(httpOptions: {} = undefined): Observable<DateModel[]> {
        let subject = new Subject<DateModel[]>();
        this.http.get<DateModel[]>(this.serviceUrl + "/api/date/getcomplexenumerable", httpOptions).subscribe((result) => {
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

    public getWrappedArray(httpOptions: {} = undefined): Observable<DateArrayWrapper> {
        let subject = new Subject<DateArrayWrapper>();
        this.http.get<DateArrayWrapper>(this.serviceUrl + "/api/date/getwrappedarray", httpOptions).subscribe((result) => {
            subject.next(result);
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWrappedModel(httpOptions: {} = undefined): Observable<DateModelWrapper> {
        let subject = new Subject<DateModelWrapper>();
        this.http.get<DateModelWrapper>(this.serviceUrl + "/api/date/getwrappedmodel", httpOptions).subscribe((result) => {
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

    public getWrappedModelList(httpOptions: {} = undefined): Observable<DateModelWrapper[]> {
        let subject = new Subject<DateModelWrapper[]>();
        this.http.get<DateModelWrapper[]>(this.serviceUrl + "/api/date/getwrappedmodellist", httpOptions).subscribe((result) => {
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

    public getWrappedModelListWrapper(httpOptions: {} = undefined): Observable<DateModelWrapperListWrapper> {
        let subject = new Subject<DateModelWrapperListWrapper>();
        this.http.get<DateModelWrapperListWrapper>(this.serviceUrl + "/api/date/getwrappedmodellistwrapper", httpOptions).subscribe((result) => {
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

    public getWrappedModelListWrapperList(httpOptions: {} = undefined): Observable<DateModelWrapperListWrapper[]> {
        let subject = new Subject<DateModelWrapperListWrapper[]>();
        this.http.get<DateModelWrapperListWrapper[]>(this.serviceUrl + "/api/date/getwrappedmodellistwrapperlist", httpOptions).subscribe((result) => {
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

    public getWrappedModelWithDate(httpOptions: {} = undefined): Observable<DateModelWrapperWithDate> {
        let subject = new Subject<DateModelWrapperWithDate>();
        this.http.get<DateModelWrapperWithDate>(this.serviceUrl + "/api/date/getwrappedmodelwithdate", httpOptions).subscribe((result) => {
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

    public getWrappedModelArray(httpOptions: {} = undefined): Observable<DateModelArrayWrapper> {
        let subject = new Subject<DateModelArrayWrapper>();
        this.http.get<DateModelArrayWrapper>(this.serviceUrl + "/api/date/getwrappedmodelarray", httpOptions).subscribe((result) => {
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

    public post(date: Date, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.post<void>(this.serviceUrl + "/api/date/post" + "?date=" + this.convertFromDate(date), httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public convertFromDate(date: Date): string {
        return date === null || date === undefined ? "" : typeof(date) === "string" ? date : date.toISOString();
    }

    public convertToDate(value: string | Date): Date {
        return typeof(value) === "string" ? new Date(value) : value;
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
