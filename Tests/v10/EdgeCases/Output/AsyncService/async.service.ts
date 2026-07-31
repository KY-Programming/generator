/* eslint-disable */
// tslint:disable

import { AsyncModel } from "./async-model";
import { SignalModel } from "./signal-model";
import { SubModel } from "./sub-model";
import { Unwrapped } from "./unwrapped";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { signal } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class AsyncService {
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

    public getAsync(httpOptions?: {}): Observable<AsyncModel> {
        let subject = new Subject<AsyncModel>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/async/getasync";
        this.http.get<AsyncModel>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getListAsync(httpOptions?: {}): Observable<AsyncModel[]> {
        let subject = new Subject<AsyncModel[]>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/async/getlistasync";
        this.http.get<AsyncModel[]>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getByIdAsync(id: number, httpOptions?: {}): Observable<AsyncModel> {
        let subject = new Subject<AsyncModel>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/async/getbyidasync";
        url = this.append(url, id, undefined, "/");
        this.http.get<AsyncModel>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getActionResultAsync(httpOptions?: {}): Observable<AsyncModel> {
        let subject = new Subject<AsyncModel>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/async/getactionresultasync";
        this.http.get<AsyncModel>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getWithoutAsyncKeyword(httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/async/getwithoutasynckeyword";
        this.http.get<string>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public postAsync(model: AsyncModel, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/async/postasync";
        this.http.post<void>(url, model, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public postWithQueryAsync(filter: string, model: AsyncModel, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/async/postwithqueryasync";
        url = this.append(url, filter, "filter");
        this.http.post<string>(url, model, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getSynchronous(httpOptions?: {}): Observable<AsyncModel> {
        let subject = new Subject<AsyncModel>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/async/getsynchronous";
        this.http.get<AsyncModel>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getSignalAsync(httpOptions?: {}): Observable<SignalModel> {
        let subject = new Subject<SignalModel>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/async/getsignalasync";
        this.http.get<Unwrapped<SignalModel>>(url, httpOptions).subscribe((result) => {
            this.convertSignalModelDate(result);
            subject.next(this.wrapSignalModel(this.fixUndefined(result)));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getSignalListAsync(httpOptions?: {}): Observable<SignalModel[]> {
        let subject = new Subject<SignalModel[]>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/async/getsignallistasync";
        this.http.get<Unwrapped<SignalModel>[]>(url, httpOptions).subscribe((result) => {
            result.forEach((m) => this.convertSignalModelDate(m));
            subject.next(this.fixUndefined(result).map((entry: Unwrapped<SignalModel>) => this.wrapSignalModel(entry)));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public updateSignalAsync(model: SignalModel, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/async/updatesignalasync";
        this.http.post<void>(url, this.unwrapSignalModel(model), httpOptions).subscribe(() => {
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

    private convertDate(value: string | Date): Date
    private convertDate(value: string | Date | undefined): Date | undefined
    private convertDate(value: string | Date | undefined): Date | undefined {
        return value === "0001-01-01T00:00:00" ? new Date("0001-01-01T00:00:00Z") : typeof(value) === "string" ? new Date(value) : value;
    }

    public wrapSignalModel(model: Unwrapped<SignalModel>): SignalModel
    public wrapSignalModel(model: Unwrapped<SignalModel> | undefined): SignalModel | undefined
    public wrapSignalModel(model: Unwrapped<SignalModel> | undefined): SignalModel | undefined {
        if (! model) {
            return undefined;
        }
        return {
            text: signal(model.text),
            number: signal(model.number),
            timestamp: signal(model.timestamp),
            optionalText: signal(model.optionalText),
            texts: signal(model.texts),
            sub: signal(this.wrapSubModel(model.sub)),
            subs: signal(model.subs?.map(entry => this.wrapSubModel(entry)))
        };
    }

    public unwrapSignalModel(model: SignalModel): Unwrapped<SignalModel>
    public unwrapSignalModel(model: SignalModel | undefined): Unwrapped<SignalModel> | undefined
    public unwrapSignalModel(model: SignalModel | undefined): Unwrapped<SignalModel> | undefined {
        if (! model) {
            return undefined;
        }
        return {
            text: model.text(),
            number: model.number(),
            timestamp: model.timestamp(),
            optionalText: model.optionalText(),
            texts: model.texts(),
            sub: this.unwrapSubModel(model.sub()),
            subs: model.subs()?.map(entry => this.unwrapSubModel(entry))
        };
    }

    public wrapSubModel(model: Unwrapped<SubModel>): SubModel
    public wrapSubModel(model: Unwrapped<SubModel> | undefined): SubModel | undefined
    public wrapSubModel(model: Unwrapped<SubModel> | undefined): SubModel | undefined {
        if (! model) {
            return undefined;
        }
        return {
            name: signal(model.name),
            changed: signal(model.changed)
        };
    }

    public unwrapSubModel(model: SubModel): Unwrapped<SubModel>
    public unwrapSubModel(model: SubModel | undefined): Unwrapped<SubModel> | undefined
    public unwrapSubModel(model: SubModel | undefined): Unwrapped<SubModel> | undefined {
        if (! model) {
            return undefined;
        }
        return {
            name: model.name(),
            changed: model.changed()
        };
    }

    public convertSignalModelDate(model?: Unwrapped<SignalModel>): void {
        if (!model) {
            return;
        }
        model.timestamp = this.convertDate(model.timestamp) ?? model.timestamp;
        this.convertSubModelDate(model.sub);
        model.subs?.forEach((m) => this.convertSubModelDate(m));
    }

    public convertSubModelDate(model?: Unwrapped<SubModel>): void {
        if (!model) {
            return;
        }
        model.changed = this.convertDate(model.changed) ?? model.changed;
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

// outputid:0f0bd27f-b1e8-4ba0-bb6e-c0f7dfef979b
