import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Permit } from '../../../models/work-order.model';

interface PermitChecklistData {
  permits: { type: string; status: string }[];
}

@Component({
  selector: 'app-work-order-permits-checklist-dialog',
  templateUrl: './work-order-permits-checklist-dialog.component.html',
  styleUrls: ['./work-order-permits-checklist-dialog.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatCheckboxModule,
    MatButtonModule,
    MatIconModule
  ]
})
export class WorkOrderPermitsChecklistDialogComponent {
  permitTypes = [
    { type: 'Initial', label: 'Initial Permit' },
    { type: 'Municipality', label: 'Baladya' },
    { type: 'RoadDepartment', label: 'Road Department' },
    { type: 'Traffic', label: 'Traffic' }
  ];
  checklist: { [type: string]: boolean } = {};

  constructor(
    public dialogRef: MatDialogRef<WorkOrderPermitsChecklistDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PermitChecklistData
  ) {
    // Initialize checklist from the predefined list, using backend data to set the status
    for (const permit of this.permitTypes) {
      const found = data.permits?.find(p => p.type === permit.type);
      this.checklist[permit.type] = found?.status?.toLowerCase() === 'approved';
    }
  }

  getIcon(checked: boolean): string {
    return checked ? 'check_circle' : 'cancel';
  }

  onSave(): void {
    // Return updated permit statuses for all predefined types
    const updatedStatuses = this.permitTypes.map(p => ({
      type: p.type,
      status: this.checklist[p.type] ? 'approved' : 'pending'
    }));
    this.dialogRef.close(updatedStatuses);
  }

  onCancel(): void {
    this.dialogRef.close();
  }
} 