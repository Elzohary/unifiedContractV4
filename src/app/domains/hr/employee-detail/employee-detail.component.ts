import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTableModule } from '@angular/material/table';
import { Employee, AttendanceRecord, Certificate, EmployeeRequest, Warning } from '../../../core/models/employee.model';
import { DataRepositoryService } from '../../../core/services/data-repository.service';

@Component({
  selector: 'app-employee-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatTabsModule,
    MatIconModule,
    MatButtonModule,
    MatDividerModule,
    MatChipsModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatMenuModule,
    MatTooltipModule,
    MatTableModule
  ],
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.scss']
})
export class EmployeeDetailComponent implements OnInit {
  employee: Employee | null = null;
  isLoading = true;
  selectedTabIndex = 0;
  
  // Tables data
  recentAttendance: AttendanceRecord[] = [];
  attendanceColumns: string[] = ['date', 'checkIn', 'checkOut', 'status', 'workHours'];
  
  certificates: Certificate[] = [];
  certificateColumns: string[] = ['name', 'issuer', 'issueDate', 'expiryDate', 'verified'];
  
  requests: EmployeeRequest[] = [];
  requestColumns: string[] = ['type', 'title', 'requestDate', 'status'];
  
  warnings: Warning[] = [];
  warningColumns: string[] = ['title', 'issueDate', 'severity', 'acknowledged'];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private dataRepository: DataRepositoryService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const employeeId = params.get('id');
      if (employeeId) {
        this.loadEmployeeData(employeeId);
      } else {
        this.router.navigate(['/hr/employees']);
      }
    });
  }

  loadEmployeeData(employeeId: string): void {
    this.isLoading = true;
    this.dataRepository.getEmployeeWithRelatedData(employeeId).subscribe({
      next: (data: { employee: Employee | undefined, attendance: AttendanceRecord[], requests: EmployeeRequest[], warnings: Warning[] }) => {
        if (data.employee) {
          this.employee = data.employee;
          this.recentAttendance = data.attendance.slice(0, 5);
          this.certificates = data.employee.certificates || [];
          this.requests = data.requests || [];
          this.warnings = data.warnings || [];
        } else {
          this.router.navigate(['/hr/employees']);
        }
        this.isLoading = false;
      },
      error: (error: any) => {
        console.error('Error loading employee', error);
        this.isLoading = false;
        this.router.navigate(['/hr/employees']);
      }
    });
  }

  editEmployee(): void {
    if (this.employee) {
      this.router.navigate(['/hr/employees', this.employee.id, 'edit']);
    }
  }

  viewRequests(): void {
    if (this.employee) {
      this.router.navigate(['/hr/employees', this.employee.id, 'requests']);
    }
  }
  
  viewAttendance(): void {
    if (this.employee) {
      this.router.navigate(['/hr/employees', this.employee.id, 'attendance']);
    }
  }
  
  viewWarnings(): void {
    if (this.employee) {
      this.router.navigate(['/hr/employees', this.employee.id, 'warnings']);
    }
  }
  
  issueWarning(): void {
    if (this.employee) {
      this.router.navigate(['/hr/warnings/new'], { 
        queryParams: { employeeId: this.employee.id } 
      });
    }
  }
  
  formatDate(date: Date | undefined): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString();
  }
  
  formatTime(date: Date | undefined): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }
  
  calculateAge(birthdate: Date): number {
    const today = new Date();
    const birthdateObj = new Date(birthdate);
    let age = today.getFullYear() - birthdateObj.getFullYear();
    const monthDiff = today.getMonth() - birthdateObj.getMonth();
    
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthdateObj.getDate())) {
      age--;
    }
    
    return age;
  }
  
  getTenureYears(joinDate: Date): number {
    const today = new Date();
    const joinDateObj = new Date(joinDate);
    let years = today.getFullYear() - joinDateObj.getFullYear();
    const monthDiff = today.getMonth() - joinDateObj.getMonth();
    
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < joinDateObj.getDate())) {
      years--;
    }
    
    return years;
  }
} 