import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private httpClient: HttpClient) { }

  logOut(): void {
    localStorage.removeItem('jwt');
    localStorage.removeItem('role');
  }

  isUserAuthenticated(): boolean {
    return localStorage.jwt !== undefined;
  }

  getToken(): string {
    return localStorage.jwt;
  }

  getRole(): string {
    return localStorage.role;
  }
}
