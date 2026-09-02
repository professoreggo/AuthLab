import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserResponseDto } from '../dtos/UserResponseDto';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  private baseUrl = 'https://localhost:7101/api/Users';

  constructor(private http: HttpClient) {}

  getUsers():Observable<UserResponseDto[]>{
    return this.http.get<UserResponseDto[]>(`${this.baseUrl}`);
  }
}
