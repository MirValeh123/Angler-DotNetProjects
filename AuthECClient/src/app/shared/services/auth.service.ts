import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {

  }

  baseUrl = 'http://localhost:5099/api';

  creatUser(formData:any){
    return this.http.post(this.baseUrl + '/signup',formData)
  }

  signIn(formData:any){
    return this.http.post(this.baseUrl + '/signin',formData)
  }
}
