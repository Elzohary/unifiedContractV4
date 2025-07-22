import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../shared/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  hidePassword = true;
  errorMessage: string | null = null;
  
  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)])
  });

  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit() {
    // If user is already logged in, redirect to dashboard or stored redirect URL
    if (this.authService.isLoggedIn) {
      this.redirectAfterLogin();
    }
  }

  handleSubmit() {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      this.errorMessage = null; // Clear previous errors
      
      this.authService.login(email!, password!).subscribe({
        next: () => {
          // Redirect after successful login
          this.redirectAfterLogin();
        },
        error: (err) => {
          this.errorMessage = err.message || 'Login failed. Please check your credentials.';
        }
      });
    } else {
      this.loginForm.markAllAsTouched();
    }
  }

  private redirectAfterLogin() {
    // If there's a stored redirect URL, go there; otherwise go to dashboard
    const redirectUrl = this.authService.redirectUrl;
    if (redirectUrl && redirectUrl !== '/login') {
      this.authService.redirectUrl = null; // Clear the stored URL
      this.router.navigateByUrl(redirectUrl);
    } else {
      this.router.navigate(['/dashboard/overview']);
    }
  }
  
  // Form error handling
  getEmailErrorMessage() {
    if (this.loginForm.controls.email.hasError('required')) {
      return 'Email is required';
    }
    return this.loginForm.controls.email.hasError('email') ? 'Not a valid email' : '';
  }
  
  getPasswordErrorMessage() {
    if (this.loginForm.controls.password.hasError('required')) {
      return 'Password is required';
    }
    return this.loginForm.controls.password.hasError('minlength') ? 'Password must be at least 6 characters' : '';
  }
}
