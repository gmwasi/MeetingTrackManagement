import { Injectable } from '@angular/core';
import {Observable} from 'rxjs/Observable';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import { Result } from './home/home.component';

@Injectable()
export class MeetingService {
  private _url = './api/Meeting';
  private _http: HttpClient;

  public constructor(http: HttpClient) {
      this._http = http;
  }

  public Post(entity: MeetingModel): Observable<Result> {
      return this._http.post<Result>(`${this._url}`, entity);
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
