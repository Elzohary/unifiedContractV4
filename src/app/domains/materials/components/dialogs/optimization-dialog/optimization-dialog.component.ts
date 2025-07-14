import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule } from '@angular/forms';

export interface OptimizationSuggestion {
  type: 'over-ordering' | 'delivery-timing' | 'missing-critical';
  material: string;
  currentQuantity?: number;
  suggestedQuantity?: number;
  potentialSavings?: number;
  reason: string;
  priority?: 'low' | 'medium' | 'high';
  deliveryDate?: string;
  workOrderStart?: string;
  daysDifference?: number;
}

export interface OptimizationDialogData {
  workOrder: any;
  suggestions: OptimizationSuggestion[];
}

@Component({
  selector: 'app-optimization-dialog',
  templateUrl: './optimization-dialog.component.html',
  styleUrls: ['./optimization-dialog.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatChipsModule,
    MatCheckboxModule,
    FormsModule
  ]
})
export class OptimizationDialogComponent {
  selectedOptimizations: boolean[] = [];

  constructor(
    public dialogRef: MatDialogRef<OptimizationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: OptimizationDialogData
  ) {
    // Initialize selection array
    this.selectedOptimizations = new Array(data.suggestions.length).fill(false);
  }

  getSuggestionIcon(type: string): string {
    const icons: Record<string, string> = {
      'over-ordering': 'warning',
      'delivery-timing': 'schedule',
      'missing-critical': 'error'
    };
    return icons[type] || 'info';
  }

  getSuggestionIconClass(type: string): string {
    return `suggestion-${type}`;
  }

  getPriorityColor(priority?: string): string {
    switch (priority) {
      case 'high': return 'warn';
      case 'medium': return 'accent';
      case 'low': return 'primary';
      default: return 'primary';
    }
  }

  hasSelectedOptimizations(): boolean {
    return this.selectedOptimizations.some(selected => selected);
  }

  getSelectedCount(): number {
    return this.selectedOptimizations.filter(selected => selected).length;
  }

  applyOptimizations(): void {
    const selectedSuggestions = this.data.suggestions.filter((_, index) => 
      this.selectedOptimizations[index]
    );

    this.dialogRef.close({
      applyOptimizations: true,
      selectedOptimizations: selectedSuggestions
    });
  }
} 