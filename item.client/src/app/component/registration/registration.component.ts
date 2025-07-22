import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user-service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.scss'
})
export class RegistrationComponent implements OnInit {

  public firstName: string = '';
  public lastName: string = '';
  public email: string = '';
  public password: string = '';
  public confirmPassword: string = '';
  private userService: UserService = inject(UserService);
  private router: Router = inject(Router);

  public ngOnInit(): void {
    const token = localStorage.getItem('authToken');
    const expiry = localStorage.getItem('expiryTime');

    // Agar token mavjud va muddati o‘tmagan bo‘lsa – login bo‘lgan
    const isTokenValid = token && expiry && +expiry > Date.now();

    this.userService.isloggedIn = !!isTokenValid;
    if (this.userService.isloggedIn) {
      this.router.navigate(['item-view']);
    }
  }
  public onRegister(): void {
    this.firstName = (document.getElementById('firstName') as HTMLInputElement)?.value;
    this.lastName = (document.getElementById('lastName') as HTMLInputElement)?.value;
    this.email = (document.getElementById('email') as HTMLInputElement)?.value;
    this.password = (document.getElementById('password') as HTMLInputElement)?.value;
    this.confirmPassword = (document.getElementById('passwordConfirm') as HTMLInputElement)?.value;

    if (this.password !== this.confirmPassword) {
      Swal.fire({
        icon: 'error',
        title: 'password to\'g\'ri kelmadi',
      });
      return;
    }

    // Faqat register muvaffaqiyatli bo‘lsa login qilinadi
    this.userService.userApiService.register(this.firstName, this.lastName, this.email, this.password)
      .subscribe({
        next: (response) => {
          // Register muvaffaqiyatli, endi login qilamiz
          this.userService.userApiService.login(this.email, this.password)
            .subscribe({
              next: (loginRes) => {
                this.userService.isloggedIn = true;
                Swal.fire({
                  icon: 'success',
                  title: 'Success!',
                  text: 'Registration successful!',
                  timer: 1500,
                  showConfirmButton: false
                });
                this.router.navigate(['item-view']);
              },
              error: (err) => {
                Swal.fire({
                  icon: 'error',
                  title: 'Registration failed',
                  text: 'Registered but login failed!',
                });
              }
            });
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Registration failed',
            text: 'Please check your input or email already registered.',
          });
        }
      });
  }
}
