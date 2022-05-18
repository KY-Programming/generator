﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated with KY.Generator 8.3.0.0
//
//      Manual changes to this file may cause unexpected behavior in your application.
//      Manual changes to this file will be overwritten if the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
/* eslint-disable */
// tslint:disable

import { ConnectionStatus } from "../models/connection-status";
import { WeatherForecast } from "../models/weather-forecast";
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
export class WeatherForecastHubService {
    private isClosed: boolean;
    public serviceUrl: string = "";
    public options: IHttpConnectionOptions;
    public logLevel: LogLevel = LogLevel.Error;
    private connection: ReplaySubject<HubConnection>;
    private readonly statusSubject: ReplaySubject<ConnectionStatus> = new ReplaySubject<ConnectionStatus>(1);
    public readonly status$: Observable<ConnectionStatus> = this.statusSubject.asObservable();
    private readonly refreshedSubject: Subject<WeatherForecast[]> = new Subject<WeatherForecast[]>();
    public readonly refreshed$: Observable<WeatherForecast[]> = this.refreshedSubject.asObservable();

    // Connects to the hub via given serviceUrl.
    // Automatically reconnects on connection loss.
    // If timeout is configured, goes to sleeping state and reconnects after the timeout
    public connect(): Observable<void> {
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
                this.statusSubject.next(ConnectionStatus.disconnected);
                subject.error(error);
            });
            return subject;
        };
        hubConnection.on("Refreshed", (forecast: WeatherForecast[]) => {
            this.refreshedSubject.next(forecast);
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

    // Send a "Refresh" message to the hub with the given parameters. Automatically connects to the hub.
    public refresh(): Observable<void> {
        let subject = new Subject<void>();
        this.connect().pipe(mergeMap(() => this.connection), take(1), mergeMap((connection) => connection.send("Refresh"))).subscribe(() => {
            subject.next();
            subject.complete();
        }, (error) => subject.error(error));
        return subject;
    }
}

// outputid:13aebde4-a1a8-4508-9cb6-7cbd78013e56
