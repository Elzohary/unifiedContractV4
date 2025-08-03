import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export interface DeleteConfirmationDialogData {
  title: string;
  message: string;
  details?: string;
  confirmButton: string;
  cancelButton: string;
}

@Component({
  selector: 'app-delete-confirmation-dialog',
  template: `
    <div class="delete-confirmation-dialog">
      <div class="dialog-header">
        <mat-icon class="warning-icon">warning</mat-icon>
        <h2 mat-dialog-title>{{ data.title }}</h2>
      </div>
      
      <mat-dialog-content>
        <p class="message">{{ data.message }}</p>
        <p class="details" *ngIf="data.details">{{ data.details }}</p>
      </mat-dialog-content>
      
      <mat-dialog-actions align="end">
        <button mat-button (click)="onCancel()">
          {{ data.cancelButton }}
        </button>
        <button mat-raised-button color="warn" (click)="onConfirm()">
          <mat-icon>delete</mat-icon>
          {{ data.confirmButton }}
        </button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .delete-confirmation-dialog {
      padding: 20px;
      max-width: 35vw;
    }

    .dialog-header {
      display: flex;
      align-items: center;
      gap: 12px;
      margin-bottom: 16px;
    }

    .warning-icon {
      color: #f44336;
      font-size: 24px;
      width: 24px;
      height: 24px;
    }

    .dialog-header h2 {
      margin: 0;
      color:rgba(244, 67, 54, 0.86);
    }

    .message {
      font-size: 16px;
      margin-bottom: 12px;
      color: #333;
    }

    .details {
      font-size: 14px;
      color: #666;
      line-height: 1.5;
      background-color: #f5f5f5;
      padding: 12px;
      border-radius: 4px;
      border-left: 4px solid #f44336;
    }

    mat-dialog-actions {
      padding: 16px 0 0 0;
      margin: 0;
    }

    mat-dialog-actions button {
      margin-left: 8px;
    }
  `],
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule
  ]
})
export class DeleteConfirmationDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteConfirmationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DeleteConfirmationDialogData
  ) {}

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }
} 