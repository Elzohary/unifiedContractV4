import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MaterialAllocationSummary } from '../../work-order-material-hub/work-order-material-hub.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-material-allocation-details-dialog',
  templateUrl: './material-allocation-details-dialog.component.html',
  styleUrls: ['./material-allocation-details-dialog.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule
  ]
})
export class MaterialAllocationDetailsDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<MaterialAllocationDetailsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: MaterialAllocationSummary
  ) {}

  close(): void {
    this.dialogRef.close();
  }
} 