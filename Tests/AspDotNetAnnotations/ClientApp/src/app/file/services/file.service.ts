/* eslint-disable */

import { FileDownload } from "../models/file-download";
import { HttpClient } from "@angular/common/http";
import { HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class FileService {
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

    public content(httpOptions?: {}): Observable<FileDownload> {
        let subject = new Subject<FileDownload>();
        httpOptions = { ...this.httpOptions, ...httpOptions, responseType: 'blob', observe: 'response' };
        let url: string = this.serviceUrl + "/file/content";
        this.http.get<HttpResponse<Blob>>(url, httpOptions).subscribe(result => {
            subject.next({ blob: result.body ?? new Blob(), fileName: this.getFileName(result.headers.get("Content-Disposition")) });
            subject.complete();
        }, error => subject.error(error));
        return subject;
    }

    public stream(httpOptions?: {}): Observable<FileDownload> {
        let subject = new Subject<FileDownload>();
        httpOptions = { ...this.httpOptions, ...httpOptions, responseType: 'blob', observe: 'response' };
        let url: string = this.serviceUrl + "/file/stream";
        this.http.get<HttpResponse<Blob>>(url, httpOptions).subscribe(result => {
            subject.next({ blob: result.body ?? new Blob(), fileName: this.getFileName(result.headers.get("Content-Disposition")) });
            subject.complete();
        }, error => subject.error(error));
        return subject;
    }

    public download(id: string, httpOptions?: {}): Observable<FileDownload> {
        let subject = new Subject<FileDownload>();
        httpOptions = { ...this.httpOptions, ...httpOptions, responseType: 'blob', observe: 'response' };
        let url: string = this.serviceUrl + "/file";
        url = this.append(url, id, undefined, "/");
        this.http.get<HttpResponse<Blob>>(url, httpOptions).subscribe(result => {
            subject.next({ blob: result.body ?? new Blob(), fileName: this.getFileName(result.headers.get("Content-Disposition")) });
            subject.complete();
        }, error => subject.error(error));
        return subject;
    }

    public export(ids: string[], httpOptions?: {}): Observable<FileDownload> {
        let subject = new Subject<FileDownload>();
        httpOptions = { ...this.httpOptions, ...httpOptions, responseType: 'blob', observe: 'response' };
        let url: string = this.serviceUrl + "/file/export";
        this.http.post<HttpResponse<Blob>>(url, ids, httpOptions).subscribe(result => {
            subject.next({ blob: result.body ?? new Blob(), fileName: this.getFileName(result.headers.get("Content-Disposition")) });
            subject.complete();
        }, error => subject.error(error));
        return subject;
    }

    public append(url: string, value: {toString(): string} | undefined | null, parameterName: string = "", separator: string = ""): string {
        if (! parameterName) {
            return url + separator + (value === null || value === undefined ? "" : encodeURIComponent(value.toString()));
        }
        if (value !== null && value !== undefined) {
            return url + (url.indexOf("?") === -1 ? "?" : "&") + parameterName + "=" + encodeURIComponent(value.toString());
        }
        return url;
    }

    private getFileName(contentDisposition: string | null): string | undefined {
        if (! contentDisposition) {
            return undefined;
        }
        let encoded = /filename\*=UTF-8''([^;]+)/i.exec(contentDisposition)?.[1];
        if (encoded) {
            return decodeURIComponent(encoded);
        }
        return /filename="?([^";]+)"?/i.exec(contentDisposition)?.[1];
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
