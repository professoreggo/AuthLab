import { Injectable } from '@angular/core';
import { RegisterDto } from '../dtos/RegisterDto';
import { UserResponseDto } from '../dtos/UserResponseDto';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = 'https://localhost:7101/api/auth'; 

  constructor(private http: HttpClient) {}

  register(dto: RegisterDto): Observable<UserResponseDto>{
    return this.http.post<UserResponseDto>(`${this.baseUrl}/register`, dto);
  }
}
