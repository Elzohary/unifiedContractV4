import { Component, Input, ChangeDetectionStrategy, OnInit, Output, EventEmitter, SimpleChanges, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { MatDialog } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { WorkOrder, SiteReport } from '../../../../models/work-order.model';
import { SiteReportFormComponent } from './site-report-form.component';
import { WorkOrderService } from '../../../../services/work-order.service';
import { SiteReportViewDialogComponent } from './site-report-view-dialog.component';

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
export class WoSiteReportTabComponent implements OnInit, OnChanges {
  @Input() workOrder!: WorkOrder;
  @Output() updated = new EventEmitter<any>();
  @Output() deleted = new EventEmitter<SiteReport>();

  constructor(
    private dialog: MatDialog,
    private workOrderService: WorkOrderService
  ) {}

  ngOnInit(): void {}

  ngOnChanges(changes: SimpleChanges): void {}

  openSampleFormDialog() {
    this.dialog.open(SiteReportFormComponent, {
      width: '600px',
      data: { workOrder: this.workOrder }
    }).afterClosed().subscribe(result => {
      if (result) {
        console.log('Sample form submitted', result);
        this.updated.emit(result); // FIX: Emit event so parent can update SSOT
      }
    });
  }

  onSiteReportFormUpdated(newReport: SiteReport) {
    this.updated.emit(newReport);
  }

  deleteReport(report: SiteReport) {
    this.deleted.emit(report);
  }

  openReport(report: SiteReport) {
    this.dialog.open(SiteReportViewDialogComponent, {
      width: '500px',
      data: { report }
    });
  }

  get groupedReports() {
    const siteReports = (this.workOrder && this.workOrder.siteReports) ? this.workOrder.siteReports : [];
    const groups: { [date: string]: { [foreman: string]: SiteReport[] } } = {};
    for (const report of siteReports) {
      const date = new Date(report.date).toLocaleDateString();
      if (!groups[date]) groups[date] = {};
      if (!groups[date][report.foremanName]) groups[date][report.foremanName] = [];
      groups[date][report.foremanName].push(report);
    }
    return groups;
  }

  get groupedReportsFlat() {
    const siteReports = (this.workOrder && this.workOrder.siteReports) ? this.workOrder.siteReports : [];
    const groups: { [date: string]: { [foreman: string]: SiteReport[] } } = {};
    for (const report of siteReports) {
      const date = new Date(report.date).toLocaleDateString();
      if (!groups[date]) groups[date] = {};
      if (!groups[date][report.foremanName]) groups[date][report.foremanName] = [];
      groups[date][report.foremanName].push(report);
    }
    // Flatten to array for template
    return Object.entries(groups).map(([date, foremen]) => ({
      date,
      foremen: Object.entries(foremen).map(([foreman, reports]) => ({
        foreman,
        reports
      }))
    })).sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
  }

  get sortedDates(): string[] {
    return Object.keys(this.groupedReports).sort((a, b) => {
      const dateA = new Date(a).getTime();
      const dateB = new Date(b).getTime();
      return dateB - dateA;
    });
  }

  getForemen(date: string): string[] {
    return Object.keys(this.groupedReports[date]);
  }
} 