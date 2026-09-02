import { Injectable } from '@angular/core';
import { RegisterDto } from '../dtos/RegisterDto';
import { UserResponseDto } from '../dtos/UserResponseDto';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginDto } from '../dtos/loginDto';
import { LoginResponse } from '../dtos/loginResponseDto';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = 'https://localhost:7101/api/auth'; 

  constructor(private http: HttpClient) {}

  register(dto: RegisterDto): Observable<UserResponseDto>{
    return this.http.post<UserResponseDto>(`${this.baseUrl}/register`, dto);
  }

  login(dto: LoginDto): Observable<LoginResponse>{
    return this.http.post<LoginResponse>(`${this.baseUrl}/login`,dto);
  }

  saveToken(token:string): void{
    localStorage.setItem('accessToken',token);
  }

  getToken(): string|null{
    return localStorage.getItem('accessToken');
  }

  logout(): void{
    localStorage.removeItem('accessToken');
  }

  isLoggedIn(): boolean{
    return !!this.getToken;
  }
  
}
