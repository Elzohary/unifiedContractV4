import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { DataRepositoryService } from '../../../core/services/data-repository.service';
import { Warning } from '../../../core/models/employee.model';
import { forkJoin, combineLatest, Subscription } from 'rxjs';
import { first, map } from 'rxjs/operators';
import { ChangeDetectorRef } from '@angular/core';
import { startOfDay, subDays } from 'date-fns';

interface Activity {
  id: string;
  title: string;
  description: string;
  date: Date;
  type: 'primary' | 'accent' | 'success' | 'warning';
  icon: string;
  time: string;
}

@Component({
  selector: 'app-hr-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatListModule,
    MatProgressBarModule,
    RouterModule
  ],
  templateUrl: './hr-dashboard.component.html',
  styleUrls: ['./hr-dashboard.component.scss']
})
export class HrDashboardComponent implements OnInit, OnDestroy {
  // Temporary route handling
  isTempRoute = false;
  tempRouteMessage = '';
  
  // Dashboard stats
  totalEmployees = 0;
  pendingRequests = 0;
  absentToday = 0;
  activeWarnings = 0;
  
  // Trend values
  prevTotalEmployees = 0;
  prevPendingRequests = 0;
  prevAbsentToday = 0;
  prevActiveWarnings = 0;

  totalEmployeesTrend = 0;
  pendingRequestsTrend = 0;
  absentTodayTrend = 0;
  activeWarningsTrend = 0;
  
  // Recent activities
  recentActivities: Activity[] = [];
  
  private employeesSub?: Subscription;
  private attendanceSubs: Subscription[] = [];

  // Add new properties for historical stats and trend labels
  previousDayStats = {
    totalEmployees: 0,
    pendingRequests: 0,
    absentToday: 0,
    activeWarnings: 0
  };
  trendLabel = 'from yesterday';

  constructor(
    private route: ActivatedRoute,
    private dataRepository: DataRepositoryService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    // Check if this is a temporary route
    this.route.data.subscribe(data => {
      if (data['tempRoute']) {
        this.isTempRoute = true;
        this.tempRouteMessage = data['message'] || 'Coming Soon';
      }
    });
    
    // Only load data if this is not a temporary route
    if (!this.isTempRoute) {
      this.loadHistoricalStats().then(() => {
        this.loadDashboardData();
      });
    }
  }

  ngOnDestroy(): void {
    this.employeesSub?.unsubscribe();
    this.attendanceSubs.forEach(sub => sub.unsubscribe());
  }

  loadDashboardData(): void {
    // Subscribe to live employees observable
    this.employeesSub = this.dataRepository.getEmployees().subscribe(employees => {
      console.log('Dashboard: Employees updated', employees);
      this.totalEmployeesTrend = this.calculateTrend(this.previousDayStats.totalEmployees, employees.length);
      this.totalEmployees = employees.length;
      this.loadRequests(employees);
      this.loadAttendanceReactive(employees);
      this.loadWarnings(employees);
      this.generateMockActivities();
    });
  }
  
  loadRequests(employees: any[]): void {
    const requests$ = employees.map(employee => 
      this.dataRepository.getRequestsByEmployee(employee.id)
    );

    forkJoin(requests$).pipe(
      first()
    ).subscribe(allRequests => {
      const pending = allRequests.reduce((total, requests) => 
        total + requests.filter((req: any) => req.status && req.status.toLowerCase() === 'pending').length, 0);
      this.pendingRequestsTrend = this.calculateTrend(this.previousDayStats.pendingRequests, pending);
      this.pendingRequests = pending;
      this.cdr.detectChanges();
    });
  }
  
  loadAttendanceReactive(employees: any[]): void {
    // Unsubscribe previous
    this.attendanceSubs.forEach(sub => sub.unsubscribe());
    this.attendanceSubs = [];
    if (employees.length === 0) {
      this.absentToday = 0;
      this.absentTodayTrend = 0;
      this.cdr.detectChanges();
      return;
    }
    // Create an array of attendance observables
    const attendanceObservables = employees.map(employee =>
      this.dataRepository.getAttendanceByEmployee(employee.id)
    );
    // Subscribe to all at once
    const sub = combineLatest(attendanceObservables).subscribe(allAttendance => {
      const today = new Date();
      let absentCount = 0;
      allAttendance.forEach((records, idx) => {
        const filtered = records.filter((r: any) => {
          const date = new Date(r.date);
          return date.getFullYear() === today.getFullYear() &&
                 date.getMonth() === today.getMonth() &&
                 date.getDate() === today.getDate();
        });
        const isAbsent = filtered.length === 0 || filtered.some((rec: any) => rec.status && rec.status.toLowerCase() === 'absent');
        if (isAbsent) {
          console.log(`Dashboard: Employee ${employees[idx].id} is absent today.`);
          absentCount++;
        }
      });
      this.absentTodayTrend = this.calculateTrend(this.previousDayStats.absentToday, absentCount);
      this.absentToday = absentCount;
      this.cdr.detectChanges();
    });
    this.attendanceSubs.push(sub);
  }
  
  loadWarnings(employees: any[]): void {
    const warnings$ = employees.map(employee =>
      this.dataRepository.getWarningsByEmployee(employee.id)
    );

    forkJoin(warnings$).pipe(
      first()
    ).subscribe(allWarnings => {
      const active = allWarnings.reduce((total, warnings) => 
        total + warnings.filter((warning: any) => 
          !warning.acknowledgement || !warning.acknowledgement.acknowledged
        ).length, 0);
      this.activeWarningsTrend = this.calculateTrend(this.previousDayStats.activeWarnings, active);
      this.activeWarnings = active;
      this.cdr.detectChanges();
    });
  }
  
  formatDateForApi(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
  
  // Generate some mock recent activities for demonstration
  generateMockActivities(): void {
    const now = new Date();
    
    this.recentActivities = [
      {
        id: '1',
        title: 'New Employee Onboarded',
        description: 'John Doe has completed the onboarding process',
        date: new Date(now.getTime() - 2 * 3600000), // 2 hours ago
        type: 'primary',
        icon: 'person_add',
        time: '2 hours ago'
      },
      {
        id: '2',
        title: 'Leave Request Approved',
        description: 'Jane Smith\'s vacation request has been approved',
        date: new Date(now.getTime() - 4 * 3600000), // 4 hours ago
        type: 'success',
        icon: 'check_circle',
        time: '4 hours ago'
      },
      {
        id: '3',
        title: 'Performance Review Scheduled',
        description: 'Mike Johnson\'s quarterly review is scheduled for next week',
        date: new Date(now.getTime() - 24 * 3600000), // 1 day ago
        type: 'accent',
        icon: 'event_note',
        time: '1 day ago'
      },
      {
        id: '4',
        title: 'Warning Issued',
        description: 'A warning has been issued to Sarah Wilson for late attendance',
        date: new Date(now.getTime() - 48 * 3600000), // 2 days ago
        type: 'warning',
        icon: 'warning',
        time: '2 days ago'
      }
    ];
  }
  
  getActivityIcon(type: string): string {
    return type === 'primary' ? 'person_add' :
           type === 'success' ? 'check_circle' :
           type === 'accent' ? 'event_note' :
           type === 'warning' ? 'warning' : 'info';
  }
  
  getActivityIconClass(type: string): string {
    return `activity-icon ${type}`;
  }

  calculateTrend(prev: number, current: number): number {
    if (prev === 0) return 0;
    return Math.round(((current - prev) / prev) * 100);
  }

  // Add a method to load yesterday's stats (mocked for now, replace with real API/storage as needed)
  async loadHistoricalStats(): Promise<void> {
    // In a real app, fetch these from a backend or local storage
    // For now, simulate with static/mock values or by recomputing from dataRepository
    const yesterday = subDays(startOfDay(new Date()), 1);
    // Example: fetch all employees/requests/attendance/warnings as of yesterday
    // This is a placeholder; replace with real logic as needed
    // For demo, just set to previous values (could be improved)
    this.previousDayStats = {
      totalEmployees: this.totalEmployees,
      pendingRequests: this.pendingRequests,
      absentToday: this.absentToday,
      activeWarnings: this.activeWarnings
    };
  }
} 