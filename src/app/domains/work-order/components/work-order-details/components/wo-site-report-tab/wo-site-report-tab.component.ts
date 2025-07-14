import { Component, Input, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { MatDialog } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { WorkOrder, SiteReport } from '../../../../models/work-order.model';
import { SiteReportFormComponent } from './site-report-form.component';

@Component({
  selector: 'app-wo-site-report-tab',
  templateUrl: './wo-site-report-tab.component.html',
  styleUrls: ['./wo-site-report-tab.component.scss'],
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatListModule,
    MatDividerModule,
    MatButtonModule,
    SiteReportFormComponent
  ]
})
export class WoSiteReportTabComponent implements OnInit {
  @Input() workOrder!: WorkOrder;
  siteReports: SiteReport[] = [];

  constructor(
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    // In a real app, fetch site reports from backend by workOrder.id
    this.siteReports = (this.workOrder as any).siteReports || [];
  }

  openSampleFormDialog() {
    this.dialog.open(SiteReportFormComponent, {
      width: '600px',
      data: { workOrder: this.workOrder }
    }).afterClosed().subscribe(result => {
      if (result) {
        // Handle the submitted form result here (e.g., add to siteReports or show a snackbar)
        console.log('Sample form submitted', result);
      }
    });
  }

  get groupedReports() {
    // Group by date, then by foreman
    const groups: { [date: string]: { [foreman: string]: SiteReport[] } } = {};
    for (const report of this.siteReports) {
      const date = new Date(report.date).toLocaleDateString();
      if (!groups[date]) groups[date] = {};
      if (!groups[date][report.foremanName]) groups[date][report.foremanName] = [];
      groups[date][report.foremanName].push(report);
    }
    return groups;
  }

  get sortedDates(): string[] {
    // Sort dates descending (latest first)
    return Object.keys(this.groupedReports).sort((a, b) => {
      // Parse as date for robust sorting
      const dateA = new Date(a).getTime();
      const dateB = new Date(b).getTime();
      return dateB - dateA;
    });
  }

  getForemen(date: string): string[] {
    return Object.keys(this.groupedReports[date]);
  }
} 