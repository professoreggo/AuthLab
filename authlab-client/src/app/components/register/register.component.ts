import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterDto } from '../../dtos/RegisterDto';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  dto: RegisterDto = { name: '', email: '', password: '' };
  message = '';

  constructor(private authService:AuthService){}
  
  onSubmit(){
    this.authService.register(this.dto).subscribe({
      next:(response)=>{
        this.message = `Registered! Id: ${response.id}`;
        console.log(response);
      },
      error:(err)=>{
        this.message = `Error: ${err.status} - ${err.message}`;
        console.error(err); 
      }
    });
  }

}
