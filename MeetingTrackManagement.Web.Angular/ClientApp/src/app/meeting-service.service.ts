import { Injectable } from '@angular/core';
import {Observable} from 'rxjs/Observable';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';

@Injectable()
export class MeetingService {
  private _url = './api/Meeting';
  private _http: HttpClient;

  public constructor(http: HttpClient) {
      this._http = http;
  }

  public Post(entity: MeetingModel): Observable<string> {
      return this._http.post<string>(`${this._url}`, entity, { responseType: 'text'});
  }

  private handleError(err: HttpErrorResponse) {
    if (err.status === 404) {
        return Observable.throw('Error occured while processing request');
    }
    return Observable.throw(err.error);
}

}

export interface MeetingModel {
  input?: string;
  tracks?: number;
}
