import { HttpClient, HttpClientCommonOptions } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

/**
 * Custom HTTP client used by the generated services instead of the default Angular HttpClient.
 * Configured in the generator via
 * .HttpClient('CustomHttpClient', '../base/custom-http-client') and the *Method() options.
 */
@Injectable({
  providedIn: 'root'
})
export class CustomHttpClient {
  private readonly http = inject(HttpClient);

  public MyGet<T>(url: string): Observable<T> {
    return this.http.get<T>(url);
  }

  public myPost<T>(url: string, body: unknown, options?: HttpClientCommonOptions): Observable<T> {
    return this.http.post<T>(url, body, options);
  }

  public myPut<T>(url: string, body: T, options?: HttpClientCommonOptions): Observable<void> {
    return this.http.put<void>(url, body, options);
  }

  public myDelete(url: string): Observable<void> {
    return this.http.delete<void>(url);
  }
}
