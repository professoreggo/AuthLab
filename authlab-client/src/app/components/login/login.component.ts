import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LoginDto } from '../../dtos/loginDto';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  dto: LoginDto = {email:'',password:''};
  errorMessage= '';

  constructor(
    private authService:AuthService,
    private router:Router
  ){}

  onSubmit(){
    this.authService.login(this.dto).subscribe({
      next:(response)=>{
        this.authService.saveToken(response.accessToken);
        this.router.navigate(['/profile']);
      },
      error:(err)=>{
        this.errorMessage = err.status ===401
        ? 'Invalid email or password'
        : 'Something went wrong';
      }
    });
  }
}
