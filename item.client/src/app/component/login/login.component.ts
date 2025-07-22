import { Component, inject, PLATFORM_ID,OnInit } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user-service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {
  public router: Router = inject(Router);
  public email: string = '';
  public password: string = '';
  private userService: UserService = inject(UserService);
  private platformId: Object = inject(PLATFORM_ID);

  public ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.userService.isloggedIn = !!localStorage.getItem('authToken');
    }
    if (this.userService.isloggedIn) {
      this.router.navigate(['item-view']);
    }
  }

  public onLogin(): void {
    const emailInput = document.getElementById('email') as HTMLInputElement;
    const passwordInput = document.getElementById('password') as HTMLInputElement;

    this.email = emailInput?.value;
    this.password = passwordInput?.value;

    var res = this.userService.userApiService.login(this.email, this.password);
    res.subscribe({
      next: (response) => {
        this.userService.isloggedIn = true;
        Swal.fire({
          icon: 'success',
          title: 'Success!',
          text: 'Login successful!',
          timer: 1000,
          showConfirmButton: false
        });
        this.router.navigate(['item-view']);
      },
      error: (err) => {
        Swal.fire({
          icon: 'error',
          title: 'Login failed',
          text: 'User not found or password incorrect',
        });
      }
    });
  }
}
