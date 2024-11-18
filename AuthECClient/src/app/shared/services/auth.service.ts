import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TOKEN_KEY } from '../constants';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}

  baseUrl = 'http://localhost:5099/api';

  creatUser(formData: any) {
    return this.http.post(this.baseUrl + '/signup', formData);
  }

  signIn(formData: any) {
    return this.http.post(this.baseUrl + '/signin', formData);
  }

  isLoggedIn() {
    return localStorage.getItem(TOKEN_KEY) != null ? true : false;
  }

  deleteToken() {
    return localStorage.removeItem(TOKEN_KEY);
  }

  saveToken(token: string) {
    return localStorage.setItem(TOKEN_KEY, token);
  }
}
