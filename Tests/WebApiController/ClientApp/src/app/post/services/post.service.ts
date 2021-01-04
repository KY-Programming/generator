// tslint:disable

import { PostModel } from "../models/post-model";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class PostService {
    private readonly http: HttpClient;
    public serviceUrl: string = "";

    public constructor(http: HttpClient) {
        this.http = http;
    }

    public postWithoutParameter(httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.post<void>(this.serviceUrl + "/post/postwithoutparameter", httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public postWithOneParameter(test: string, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.post<void>(this.serviceUrl + "/post/postwithoneparameter" + "?test=" + this.convertAny(test), httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public postWithTwoParameter(text: string, count: number, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.post<void>(this.serviceUrl + "/post/postwithtwoparameter" + "?text=" + this.convertAny(text) + "&count=" + this.convertAny(count), httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public postWithBodyParameter(model: PostModel, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.post<void>(this.serviceUrl + "/post/postwithbodyparameter", model, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public postWithValueAndBodyParameter(id: number, model: PostModel, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.post<void>(this.serviceUrl + "/post/postwithvalueandbodyparameter" + "?id=" + this.convertAny(id), model, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public postWithValueAndBodyParameterFlipped(model: PostModel, id: number, httpOptions: {} = undefined): Observable<void> {
        let subject = new Subject<void>();
        this.http.post<void>(this.serviceUrl + "/post/postwithvalueandbodyparameterflipped" + "?id=" + this.convertAny(id), model, httpOptions).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }

    public convertAny(value: any): string {
        return value === null || value === undefined ? "" : value.toString();
    }
}

// outputid:627408ca-a818-4326-b843-415f5bbfb028
