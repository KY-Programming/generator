import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export class CustomHttpClient extends HttpClient {
  public MyGet<T>(url: string): Observable<T> {
    return this.get<T>(url);
  }

  public myPost<T>(url: string, body: any | null, options?: unknown): Observable<T> {
    return this.post<T>(url, body, options);
  }

  public myPatch<TParameter, TReturn>(url: string, body: TParameter, options?: unknown): Observable<TReturn> {
    return this.patch<TReturn>(url, body, options);
  }

  public myPut<T>(url: string, body: T, options?: unknown): Observable<void> {
    return this.put<void>(url, body, options);
  }

  public myDelete(url: string, id: string, options?: unknown): Observable<void> {
    return this.delete<void>(url + '/' + id, options);
  }
}
