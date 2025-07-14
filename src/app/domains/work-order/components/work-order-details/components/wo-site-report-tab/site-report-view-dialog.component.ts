import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { SiteReport } from '../../../../models/work-order.model';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-site-report-view-dialog',
  template: `
    <div class="dialog-root">
      <div class="dialog-header">
        <h2>Site Report Details</h2>
        <button mat-icon-button mat-dialog-close aria-label="Close">
          <mat-icon>close</mat-icon>
        </button>
      </div>
      <div class="section summary-section">
        <div class="summary-row">
          <mat-icon class="icon">person</mat-icon>
          <span class="label">Added by:</span>
          <span class="value">{{ data.report.foremanName }}</span>
        </div>
        <div class="summary-row">
          <mat-icon class="icon">event</mat-icon>
          <span class="label">Date:</span>
          <span class="value">{{ data.report.date | date:'mediumDate' }}</span>
        </div>
        <div class="summary-row">
          <mat-icon class="icon">schedule</mat-icon>
          <span class="label">Time:</span>
          <span class="value">{{ data.report.createdAt | date:'shortTime' }}</span>
        </div>
        <div class="summary-row work-done-row">
          <mat-icon class="icon">engineering</mat-icon>
          <span class="label">Work Done:</span>
          <span class="value work-done-value">{{ data.report.workDone }}</span>
        </div>
        <div class="summary-row" *ngIf="data.report.notes">
          <mat-icon class="icon">notes</mat-icon>
          <span class="label">Notes:</span>
          <span class="value">{{ data.report.notes }}</span>
        </div>
      </div>

      <div class="section">
        <div class="section-title">Work Done</div>
        <table mat-table [dataSource]="[data.report]" class="mat-elevation-z1 work-done-table">
          <ng-container matColumnDef="workDone">
            <th mat-header-cell *matHeaderCellDef>Work Description</th>
            <td mat-cell *matCellDef="let report">{{ report.workDone }}</td>
          </ng-container>
          <ng-container matColumnDef="quantity">
            <th mat-header-cell *matHeaderCellDef>Quantity</th>
            <td mat-cell *matCellDef="let report">{{ report.actualQuantity !== undefined ? report.actualQuantity : '-' }}</td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="['workDone', 'quantity']"></tr>
          <tr mat-row *matRowDef="let row; columns: ['workDone', 'quantity'];"></tr>
        </table>
      </div>

      <div class="section" *ngIf="data.report.materialsUsed?.length">
        <div class="section-title">Materials Used</div>
        <table mat-table [dataSource]="data.report.materialsUsed" class="mat-elevation-z1 materials-table">
          <ng-container matColumnDef="materialName">
            <th mat-header-cell *matHeaderCellDef>Material Name</th>
            <td mat-cell *matCellDef="let m">{{ m.materialName }}</td>
          </ng-container>
          <ng-container matColumnDef="quantity">
            <th mat-header-cell *matHeaderCellDef>Quantity</th>
            <td mat-cell *matCellDef="let m">{{ m.quantity }}</td>
          </ng-container>
          <ng-container matColumnDef="unit">
            <th mat-header-cell *matHeaderCellDef>Unit</th>
            <td mat-cell *matCellDef="let m">{{ m.unit || '-' }}</td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="['materialName', 'quantity', 'unit']"></tr>
          <tr mat-row *matRowDef="let row; columns: ['materialName', 'quantity', 'unit'];"></tr>
        </table>
      </div>

      <div class="section" *ngIf="data.report.photos?.length">
        <div class="section-title">Photos</div>
        <div class="photo-grid">
          <a *ngFor="let photo of data.report.photos" [href]="photo.url" target="_blank" class="photo-link">
            <img [src]="photo.url" [alt]="photo.caption || 'Site Photo'" class="photo-thumb" />
          </a>
        </div>
      </div>
      <div class="section" *ngIf="!data.report.photos?.length">
        <div class="section-title">Photos</div>
        <div class="photo-placeholder">No photos attached</div>
      </div>

      <div class="dialog-actions">
        <button mat-button mat-dialog-close>Close</button>
      </div>
    </div>
  `,
  styles: [`
    .dialog-root {
      min-width: 350px;
      max-width: 540px;
      padding: 0;
      background: #fff;
      border-radius: 10px;
      overflow: hidden;
    }
    .dialog-header {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 18px 24px 8px 24px;
      border-bottom: 1px solid #e0e0e0;
    }
    .dialog-header h2 {
      margin: 0;
      font-size: 1.35em;
      font-weight: 700;
      color: #222;
    }
    .section {
      padding: 18px 24px 0 24px;
    }
    .summary-section {
      padding-bottom: 0;
    }
    .summary-row {
      display: flex;
      align-items: center;
      margin-bottom: 8px;
      font-size: 1em;
    }
    .summary-row .icon {
      font-size: 1.1em;
      margin-right: 7px;
      color: #1976d2;
    }
    .label {
      font-weight: 600;
      color: #1976d2;
      margin-right: 6px;
      min-width: 90px;
    }
    .value {
      color: #222;
      font-weight: 400;
    }
    .work-done-row .work-done-value {
      font-weight: 600;
      color: #333;
      font-size: 1.08em;
    }
    .section-title {
      font-weight: 700;
      color: #444;
      margin-bottom: 8px;
      font-size: 1.08em;
      margin-top: 8px;
    }
    table {
      width: 100%;
      margin-bottom: 12px;
      background: #fafbfc;
      border-radius: 6px;
      overflow: hidden;
    }
    th.mat-header-cell, td.mat-cell {
      padding: 8px 10px;
      font-size: 0.98em;
    }
    th.mat-header-cell {
      background: #f5f7fa;
      color: #1976d2;
      font-weight: 600;
      border-bottom: 1.5px solid #e0e0e0;
    }
    td.mat-cell {
      color: #333;
      border-bottom: 1px solid #f0f0f0;
    }
    .photo-grid {
      display: flex;
      flex-wrap: wrap;
      gap: 10px;
      margin-top: 2px;
      margin-bottom: 10px;
    }
    .photo-link {
      display: block;
      border-radius: 6px;
      border: 1.5px solid #e0e0e0;
      overflow: hidden;
      transition: box-shadow 0.2s, border-color 0.2s;
    }
    .photo-link:hover {
      border-color: #1976d2;
      box-shadow: 0 2px 8px rgba(25, 118, 210, 0.12);
    }
    .photo-thumb {
      width: 70px;
      height: 70px;
      object-fit: cover;
      display: block;
      background: #fafafa;
    }
    .photo-placeholder {
      color: #bbb;
      font-style: italic;
      font-size: 0.98em;
      padding: 8px 0 0 0;
    }
    .dialog-actions {
      display: flex;
      justify-content: flex-end;
      padding: 18px 24px 18px 24px;
      border-top: 1px solid #e0e0e0;
      background: #fafbfc;
      margin-top: 18px;
    }
  `],
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatTableModule, MatIconModule, MatButtonModule   ]
})
export class SiteReportViewDialogComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { report: SiteReport },
    public dialogRef: MatDialogRef<SiteReportViewDialogComponent>
  ) {}
} 