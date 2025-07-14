import { Component, Input, ChangeDetectionStrategy, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatListModule } from '@angular/material/list';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DocumentService, UploadedDocument } from 'src/app/shared/services/document.service';
import { Permit, WorkOrder } from '../../../../models/work-order.model';
import { MatExpansionModule } from '@angular/material/expansion';

@Component({
  selector: 'app-wo-documents-tab',
  templateUrl: './wo-documents-tab.component.html',
  styleUrls: ['./wo-documents-tab.component.scss'],
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatListModule,
    MatExpansionModule
  ]
})
export class WoDocumentsTabComponent implements OnInit, OnChanges {
  @Input() workOrder!: WorkOrder;
  uploading: { [permitId: string]: boolean } = {};
  uploadProgress: { [permitId: string]: number } = {};
  documents: { [permitId: string]: UploadedDocument[] } = {};

  constructor(private documentService: DocumentService, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.refreshAllPermitDocuments();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['workOrder']) {
      this.refreshAllPermitDocuments();
    }
  }

  get approvedPermits(): Permit[] {
    return (this.workOrder?.permits || []).filter((p: Permit) => p.status === 'approved');
  }

  refreshAllPermitDocuments() {
    this.approvedPermits.forEach(permit => this.refreshDocuments(permit.id));
  }

  refreshDocuments(permitId: string) {
    this.documentService.getDocumentsByEntity('permit', permitId).subscribe(docs => {
      this.documents[permitId] = docs;
    });
  }

  onFileSelected(event: Event, permit: Permit) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const files = Array.from(input.files);
      this.uploading[permit.id] = true;
      this.uploadProgress[permit.id] = 0;
      this.documentService.uploadMultipleFiles(files, 'permit', permit.id, { workOrderId: this.workOrder.id })
        .subscribe({
          next: () => {
            this.snackBar.open('File(s) uploaded', 'Close', { duration: 2000 });
            this.refreshDocuments(permit.id);
            this.uploading[permit.id] = false;
          },
          error: () => {
            this.snackBar.open('Upload failed', 'Close', { duration: 3000 });
            this.uploading[permit.id] = false;
          }
        });
    }
  }

  removeDocument(doc: UploadedDocument, permit: Permit) {
    this.documentService.deleteDocument(doc.id).subscribe(success => {
      if (success) {
        this.snackBar.open('File deleted', 'Close', { duration: 2000 });
        this.refreshDocuments(permit.id);
      } else {
        this.snackBar.open('Delete failed', 'Close', { duration: 3000 });
      }
    });
  }
} 