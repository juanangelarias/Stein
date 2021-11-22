import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError, Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";

import { ErrorMessage } from "../model/errorMessage";
import { inventory } from '../model/inventory';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private baseUrl: string = "https://localhost:5001/api/inventory";

  constructor(private http: HttpClient) { }

  getAll():Observable<inventory[]>{
    let headers = this.getHeaders()

    return this.http
      .get<inventory[]>(this.baseUrl)
      .pipe(catchError(this.handleError));
  }

  private getHeaders(): HttpHeaders {
    let headers = new HttpHeaders({'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': '*',
    'Access-Control-Allow-Credentials': 'true',
    'Access-Control-Allow-Headers': 'Content-Type',
    'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE'});
    
    //();
    //headers = headers
    //  .set("Content-Type", "application/json")
    //  .set("Cache-Control", "no-cache")
    //  .set("Pragma", "no-cache")
    //  .set("Expires", "Sat, 01 Jan 2000 00:00:00 GMT")
    //  .set("If-Modified-Since", "0");

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
