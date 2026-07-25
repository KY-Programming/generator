/* eslint-disable */
// tslint:disable

import { PlainModel } from "./plain-model";
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
export class SignalsService {
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

    public get(httpOptions?: {}): Observable<SignalModel> {
        let subject = new Subject<SignalModel>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/signals/get";
        this.http.get<Unwrapped<SignalModel>>(url, httpOptions).subscribe((result) => {
            this.convertSignalModelDate(result);
            subject.next(this.wrapSignalModel(this.fixUndefined(result)));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getAll(httpOptions?: {}): Observable<SignalModel[]> {
        let subject = new Subject<SignalModel[]>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/signals/getall";
        this.http.get<Unwrapped<SignalModel>[]>(url, httpOptions).subscribe((result) => {
            result.forEach((m) => this.convertSignalModelDate(m));
            subject.next(this.fixUndefined(result).map((entry: Unwrapped<SignalModel>) => this.wrapSignalModel(entry)));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public update(model: SignalModel, httpOptions?: {}): Observable<string> {
        let subject = new Subject<string>();
        httpOptions = { responseType: 'text', ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/signals/update";
        this.http.post<string>(url, this.unwrapSignalModel(model), httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public updateAll(models: SignalModel[], httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/signals/updateall";
        this.http.post<void>(url, models.map((entry) => this.unwrapSignalModel(entry)), httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public getPlain(httpOptions?: {}): Observable<PlainModel> {
        let subject = new Subject<PlainModel>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/signals/getplain";
        this.http.get<PlainModel>(url, httpOptions).subscribe((result) => {
            subject.next(this.fixUndefined(result));
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public updatePlain(model: PlainModel, httpOptions?: {}): Observable<void> {
        let subject = new Subject<void>();
        httpOptions = { ...this.httpOptions, ...httpOptions};
        let url: string = this.serviceUrl + "/api/v1/signals/updateplain";
        this.http.post<void>(url, model, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
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

// outputid:b574b782-3e0e-4f8c-8378-106e837ed00d
