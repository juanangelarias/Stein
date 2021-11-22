import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError, Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";

import { Request } from '../model/request';
import { ErrorMessage } from "../model/errorMessage";
import { RequestProcessResult } from '../model/RequestProcessResult';

@Injectable({
  providedIn: 'root'
})
export class RequestService {
  private baseUrl: string = "https://localhost:5001/api/request";

  constructor(private http: HttpClient) { }

  getAll():Observable<Request[]>{
    let headers = this.getHeaders()

    return this.http
      .get<Request[]>(this.baseUrl)
      .pipe(catchError(this.handleError));
  }  

  process(request: Request):Observable<RequestProcessResult>{
    let headers = this.getHeaders()

    let url = `${this.baseUrl}/${request.id}`;
    return this.http
      .post<RequestProcessResult>(url, request, {headers})
      .pipe(catchError(this.handleError));
  }

  private getHeaders(): HttpHeaders {
    let headers = new HttpHeaders();
    headers = headers
      .set("Current-Type", "application/json")
      .set('Cache-Control', 'no-cache')
      .set('Pragma', 'no-cache')
      .set('Expires', 'Sat, 01 Jan 2000 00:00:00 GMT')
      .set('If-Modified-Since', '0');

    return headers;
  }

  private handleError(err: HttpErrorResponse) {
    let errorMessage = "";
    if (err.error instanceof ErrorEvent) {
      errorMessage = `An error occurred: ${err.error.message}`;
    } else if (err.status === 400 || err.status === 401) {
      let errmsg: ErrorMessage = err.error;
      errorMessage = errmsg.error;
    } else {
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }

    console.error(errorMessage);
    return throwError(errorMessage);
  }
}
