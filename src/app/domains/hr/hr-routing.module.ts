import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Import HR components 
import { HrDashboardComponent } from './hr-dashboard/hr-dashboard.component';
import { EmployeeListComponent } from './employee/employee-list/employee-list.component';
import { EmployeeDetailComponent } from './employee/employee-detail/employee-detail.component';
import { EmployeeFormComponent } from './employee/employee-form/employee-form.component';
import { AttendanceManagementComponent } from './attendance/attendance-management/attendance-management.component';
import { EmployeeRequestsComponent } from './requests/employee-requests/employee-requests.component';
import { HrRoleGuard } from './guards/hr-role.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    component: HrDashboardComponent,
    canActivate: [HrRoleGuard]
  },
  {
    path: 'employees',
    component: EmployeeListComponent,
    canActivate: [HrRoleGuard]
  },
  {
    path: 'employees/new',
    component: EmployeeFormComponent,
    canActivate: [HrRoleGuard]
  },
  {
    path: 'employees/:id',
    component: EmployeeDetailComponent,
    canActivate: [HrRoleGuard]
  },
  {
    path: 'employees/:id/edit',
    component: EmployeeFormComponent,
    canActivate: [HrRoleGuard]
  },
  {
    path: 'employees/:id/requests',
    component: EmployeeRequestsComponent,
    canActivate: [HrRoleGuard]
  },
  {
    path: 'employees/:id/attendance',
    component: AttendanceManagementComponent,
    canActivate: [HrRoleGuard]
  },
  {
    path: 'employees/:id/warnings',
    component: HrDashboardComponent,
    data: { tempRoute: true, message: 'Employee Warnings Management - Coming Soon' },
    canActivate: [HrRoleGuard]
  },
  {
    path: 'requests',
    component: EmployeeRequestsComponent,
    canActivate: [HrRoleGuard]
  },
  {
    path: 'attendance',
    component: AttendanceManagementComponent,
    canActivate: [HrRoleGuard]
  },
  {
    path: 'warnings',
    component: HrDashboardComponent,
    data: { tempRoute: true, message: 'Employee Warnings Management - Coming Soon' },
    canActivate: [HrRoleGuard]
  },
  {
    path: 'announcements',
    component: HrDashboardComponent,
    data: { tempRoute: true, message: 'HR Announcements - Coming Soon' },
    canActivate: [HrRoleGuard]
  },
  {
    path: 'reports',
    component: HrDashboardComponent,
    data: { tempRoute: true, message: 'HR Reports - Coming Soon' },
    canActivate: [HrRoleGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HrRoutingModule { } 