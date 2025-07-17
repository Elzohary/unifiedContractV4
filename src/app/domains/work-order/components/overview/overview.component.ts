import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { StatsCardComponent } from '../../../../shared/components/stats-card/stats-card.component';
import { UnderConstructionComponent } from '../../../../shared/components/under-construction/under-construction.component';
import { WorkOrderService } from '../../services/work-order.service';
import { ActivityLogService, ActivityLog } from '../../../../shared/services/activity-log.service';
import { WorkOrderDashboardViewModel, WorkOrderDashboardStats } from '../../viewModels/work-order-dashboard.viewmodel';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-overview',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatGridListModule,
    MatButtonModule,
    MatIconModule,
    MatTabsModule,
    StatsCardComponent,
    UnderConstructionComponent
  ],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.scss'
})
export class OverviewComponent implements OnInit {
  stats$: Observable<WorkOrderDashboardStats>;
  recentActivities$: Observable<ActivityLog[]>;

  constructor(
    private dashboardVM: WorkOrderDashboardViewModel,
    private activityLogService: ActivityLogService
  ) {
    this.stats$ = this.dashboardVM.stats$;
    this.recentActivities$ = this.activityLogService.getAllActivityLogs();
  }

  ngOnInit(): void {}

  getActivityIcon(action: string): string {
    switch (action) {
      case 'create':
        return 'add_circle';
      case 'update':
        return 'edit';
      case 'delete':
        return 'delete';
      case 'complete':
        return 'task_alt';
      case 'assign':
        return 'person_add';
      default:
        return 'notifications';
    }
  }

  getIconColorClass(action: string): string {
    switch (action) {
      case 'create':
        return 'icon-primary';
      case 'update':
        return 'icon-info';
      case 'delete':
        return 'icon-danger';
      case 'complete':
        return 'icon-success';
      case 'assign':
        return 'icon-warning';
      default:
        return 'icon-default';
    }
  }

  // Add this method to filter recent activities (last 12 hours)
  isRecentActivity(activity: ActivityLog): boolean {
    const twelveHoursAgo = new Date();
    twelveHoursAgo.setHours(twelveHoursAgo.getHours() - 12);
    return new Date(activity.timestamp) >= twelveHoursAgo;
  }
}
