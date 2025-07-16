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
    if (!this.workOrder?.id) return;
    this.workOrderService.getWorkOrderById(this.workOrder.id).subscribe((latestWorkOrder) => {
      if (!latestWorkOrder) return;
      this.dialog.open(SiteReportFormComponent, {
        width: '600px',
        data: { workOrder: latestWorkOrder }
      }).afterClosed().subscribe(result => {
        if (result) {
          console.log('Sample form submitted', result);
          this.updated.emit(result);
        }
      });
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
      data: { report, items: this.workOrder?.items || [] }
    });
  }

  continueReport(report: SiteReport) {
    // Determine the first incomplete step
    let stepIndex = 0;
    const hasSafety = report.photos?.some(p => p.category === 'safety');
    const hasProgress = report.photos?.some(p => p.category === 'progress');
    const hasHousekeeping = report.photos?.some(p => p.category === 'housekeeping');
    if (!hasSafety) stepIndex = 0;
    else if (!hasProgress) stepIndex = 1;
    else if (!hasHousekeeping) stepIndex = 2;
    else stepIndex = 3; // All steps filled, go to review
    this.dialog.open(SiteReportFormComponent, {
      width: '600px',
      data: {
        workOrder: this.workOrder,
        report,
        startStep: stepIndex
      }
    }).afterClosed().subscribe(result => {
      if (result) {
        this.updated.emit(result);
      }
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
    // Sort each group by newest first (descending by date/time)
    Object.values(groups).forEach(foremen => {
      Object.values(foremen).forEach(reportsArr => {
        reportsArr.sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
      });
    });
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
    // Sort each group by newest first (descending by date/time)
    Object.values(groups).forEach(foremen => {
      Object.values(foremen).forEach(reportsArr => {
        reportsArr.sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
      });
    });
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

  // Add summary method for displaying a brief summary of the report
  getReportSummary(report: SiteReport): string {
    if (!this.workOrder?.items?.length) return '';
    const item = this.workOrder.items.find(i => i.id === report.workDone);
    const desc = item?.itemDetail?.shortDescription || report.workDone;
    const qty = report.actualQuantity !== undefined ? `Qty: ${report.actualQuantity}` : '';
    return `${desc}${qty ? ' | ' + qty : ''}`;
  }
} 