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
  generalAttachments: UploadedDocument[] = [];
  permitDocuments: { [key: string]: UploadedDocument[] } = {};
  uploading: { [key: string]: boolean } = {};
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
    return (this.workOrder?.permits || []).filter((p: Permit) => p.status?.toLowerCase() === 'approved');
  }

  refreshAllPermitDocuments() {
    this.approvedPermits.forEach(permit => this.refreshDocuments(permit.id));
  }

  refreshDocuments(permitId: string) {
    this.documentService.getDocumentsByEntity('permit', permitId).subscribe(docs => {
      this.documents[permitId] = docs;
    });
  }

  onFileSelected(event: Event, permitType: string): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      this.documentService.uploadDocument(file, this.workOrder.id, 'WorkOrder').subscribe({
        next: (uploadedDoc) => {
          console.log('General document upload successful', uploadedDoc);
        },
        error: (err) => {
          this.snackBar.open('Upload failed', 'Close', { duration: 3000 });
          this.uploading[permitType] = false;
        }
      });
    }
  }

  removeGeneralDocument(doc: UploadedDocument): void {
    this.documentService.deleteDocument(doc.id).subscribe(success => {
      if (success) {
        this.generalAttachments = this.generalAttachments.filter(d => d.id !== doc.id);
      }
    });
  }

  removePermitDocument(doc: UploadedDocument, permitType: string): void {
    this.documentService.deleteDocument(doc.id).subscribe(success => {
      if (success) {
        this.permitDocuments[permitType] = this.permitDocuments[permitType].filter(d => d.id !== doc.id);
        this.snackBar.open('Document deleted', 'Close', { duration: 2000 });
      } else {
        this.snackBar.open('Failed to delete document', 'Close', { duration: 3000 });
      }
    });
  }

  loadPermitDocuments(permitId: string, permitType: string): void {
    this.documentService.getDocumentsByEntity('permit', permitId).subscribe(docs => {
      this.permitDocuments[permitType] = docs;
    });
  }
} 