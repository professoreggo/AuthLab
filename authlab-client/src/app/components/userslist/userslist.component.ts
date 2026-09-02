import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { UserResponseDto } from '../../dtos/UserResponseDto';
import { UsersService } from '../../services/users.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-userslist',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './userslist.component.html',
  styleUrl: './userslist.component.css'
})
export class UserslistComponent implements OnInit {

  users: UserResponseDto[] =[];

  constructor(
    private usersService: UsersService,
    private authService: AuthService,
    private router:Router
    ){}

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(): void{
    this.usersService.getUsers().subscribe({
      next:(res)=>{
        this.users=res;
      },
      error:(err)=>{
        console.error(err);
      }
    });
  }
  onLogout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  




}
