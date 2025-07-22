import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatBadgeModule } from '@angular/material/badge';
import { MatMenuModule } from '@angular/material/menu';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ThemeService } from '../../../../../shared/services/theme.service';
import { AuthService, AuthUser } from '../../../../../shared/services/auth.service';

@Component({
  selector: 'app-header-dashboard-management',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatMenuModule,
    MatBadgeModule,
    FormsModule
  ],
  templateUrl: './header-dashboard-management.component.html',
  styleUrl: './header-dashboard-management.component.scss'
})
export class HeaderDashboardManagementComponent implements OnInit {
  private _formBuilder = inject(FormBuilder);
  private _themeService = inject(ThemeService);
  private _authService = inject(AuthService);
  private _router = inject(Router);

  isDark = false;
  currentUser: AuthUser | null = null;
  
  formGroup = this._formBuilder.group({
    enableWifi: '',
    acceptTerms: ['', Validators.requiredTrue],
  });

  ngOnInit(): void {
    // Initialize isDark from the theme service
    this.isDark = this._themeService.isDarkTheme();
    
    // Subscribe to theme changes
    this._themeService.theme$.subscribe(theme => {
      this.isDark = theme === 'dark-theme';
    });

    // Subscribe to user changes
    this._authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });

    // Set initial user value
    this.currentUser = this._authService.currentUserValue;
  }

  alertFormValues(formGroup: FormGroup) {
    alert(JSON.stringify(formGroup.value, null, 2));
  }

  toggleTheme() {
    this._themeService.toggleTheme();
  }

  logout() {
    this._authService.logout().subscribe(() => {
      this._router.navigate(['/login']);
    });
  }

  get displayName(): string {
    if (this.currentUser) {
      return this.currentUser.name || this.currentUser.email || 'User';
    }
    return 'User';
  }
}
