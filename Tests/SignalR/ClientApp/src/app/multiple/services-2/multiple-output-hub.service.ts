/* eslint-disable */
// tslint:disable

import { ConnectionStatus } from "../models-2/connection-status";
import { Injectable } from "@angular/core";
import { HubConnection } from "@microsoft/signalr";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { IHttpConnectionOptions } from "@microsoft/signalr";
import { LogLevel } from "@microsoft/signalr";
import { Observable } from "rxjs";
import { filter } from "rxjs/operators";
import { map } from "rxjs/operators";
import { mergeMap } from "rxjs/operators";
import { take } from "rxjs/operators";
import { ReplaySubject } from "rxjs";
import { Subject } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class MultipleOutputHubService {
    private isClosed: boolean;
    public serviceUrl: string = "";
    public options: IHttpConnectionOptions;
    public logLevel: LogLevel = LogLevel.Error;
    private connection: ReplaySubject<HubConnection>;
    private readonly timeouts: number[] = [0, 0, 1000, 2000, 5000];
    private readonly statusSubject: ReplaySubject<ConnectionStatus> = new ReplaySubject<ConnectionStatus>(1);
    public readonly status$: Observable<ConnectionStatus> = this.statusSubject.asObservable();
    private readonly test2Subject: Subject<void> = new Subject<void>();
    public readonly test2$: Observable<void> = this.test2Subject.asObservable();

    // Connects to the hub via given serviceUrl.
    // Automatically reconnects on connection loss.
    // If timeout is configured, goes to sleeping state and reconnects after the timeout
    public connect(trial: number = 0): Observable<void> {
        if (! this.serviceUrl) {
            throw new Error("serviceUrl can not be empty. Set it via service.serviceUrl.")
        }
        if (this.connection && ! this.isClosed) {
            return this.status$.pipe(filter((status) => status === ConnectionStatus.connected), take(1), map(() => {
            }));
        }
        this.isClosed = false;
        this.connection = this.connection ? this.connection : new ReplaySubject<HubConnection>(1);
        let hubConnection: HubConnection = new HubConnectionBuilder().withUrl(this.serviceUrl, this.options).configureLogging(this.logLevel).build();
        let startConnection: () => Observable<void> = () => {
            this.statusSubject.next(ConnectionStatus.connecting);
            let subject = new Subject<void>();
            hubConnection.start().then(() => {
                subject.next();
                subject.complete();
                this.statusSubject.next(ConnectionStatus.connected);
            }).catch((error) => {
                if (this.isClosed) {
                    return;
                }
                this.statusSubject.next(ConnectionStatus.sleeping);
                let timeout: number = this.timeouts[trial];
                trial++;
                timeout = timeout || this.timeouts[this.timeouts.length - 1] || 0;
                setTimeout(() => {
                    if (this.isClosed) {
                        return;
                    }
                    startConnection().subscribe(() => {
                        subject.next();
                        subject.complete();
                    }, (innerError) => subject.error(innerError))
                }, timeout);
            });
            return subject;
        };
        hubConnection.on("Test2", () => {
            this.test2Subject.next();
        });
        hubConnection.onclose(() => {
            if (! this.isClosed) {
                startConnection();
            }
        });
        this.connection.next(hubConnection);
        return startConnection();
    }

    // Close an active connection to the hub.
    // If the service is reconnecting/sleeping the connection attempt will be canceled
    public disconnect(): void {
        this.isClosed = true;
        this.connection?.pipe(take(1)).subscribe((hubConnection) => {
            hubConnection.stop().then(() => {
                this.statusSubject.next(ConnectionStatus.disconnected);
            });
        });
    }

    // Send a "Test" message to the hub with the given parameters. Automatically connects to the hub.
    public test(): Observable<void> {
        let subject = new Subject<void>();
        this.connect().pipe(mergeMap(() => this.connection), take(1), mergeMap((connection) => connection.send("Test"))).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }
}

// outputid:969bbdc9-df23-40ed-b6dd-702aa1fe7b03
