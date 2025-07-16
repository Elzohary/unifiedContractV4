import { Component, EventEmitter, Input, Output, SimpleChanges, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-photo-upload',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatTooltipModule
  ],
  template: `
    <div class="photo-upload-container" (drop)="onDrop($event)" (dragover)="onDragOver($event)">
      <input type="file" #fileInput (change)="onFilesSelected($event)" accept="image/*" [multiple]="multiple" [disabled]="disabled" style="display: none;" />
      <button mat-stroked-button type="button" (click)="fileInput.click()" class="upload-button">
        <mat-icon>add_photo_alternate</mat-icon>
        {{buttonLabel}}
      </button>
      <span class="drag-hint">or drag & drop images here</span>
      <div class="photo-thumbnails">
        <mat-chip *ngFor="let file of files; let i = index" removable (removed)="removeFile(i)">
          <img *ngIf="file.preview" [src]="file.preview" class="thumbnail" alt="Photo preview" />
          <mat-icon *ngIf="!file.preview">image</mat-icon>
          {{ file.name }}
          <mat-icon matChipRemove>cancel</mat-icon>
        </mat-chip>
      </div>
    </div>
  `,
  styles: [`
    .photo-upload-container {
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      gap: 8px;
      border: 2px dashed #e0e0e0;
      border-radius: 8px;
      padding: 16px;
      background: #fafafa;
      position: relative;
      min-width: 280px;
      min-height: 120px;
    }
    .upload-button {
      margin-bottom: 8px;
    }
    .drag-hint {
      font-size: 13px;
      color: #888;
      margin-bottom: 8px;
    }
    .photo-thumbnails {
      display: flex;
      flex-wrap: wrap;
      gap: 8px;
      margin-top: 8px;
    }
    .thumbnail {
      width: 32px;
      height: 32px;
      object-fit: cover;
      border-radius: 4px;
      margin-right: 6px;
      vertical-align: middle;
    }
  `]
})
export class PhotoUploadComponent implements OnChanges {
  @Input() buttonLabel = 'Add Photos';
  @Input() maxFiles = 10;
  @Input() maxSizeMB = 5;
  @Input() required = false;
  @Input() multiple = true;
  @Input() existingFiles: File[] = [];
  @Input() disabled = false;
  @Output() filesChange = new EventEmitter<File[]>();

  files: (File & { preview?: string })[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['existingFiles']) {
      this.files = (this.existingFiles || []).map(f => f);
      this.filesChange.emit(this.files);
    }
  }

  onFilesSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      this.addFiles(Array.from(input.files));
      input.value = '';
    }
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    if (event.dataTransfer && event.dataTransfer.files) {
      this.addFiles(Array.from(event.dataTransfer.files));
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
  }

  addFiles(newFiles: File[]): void {
    for (const file of newFiles) {
      if (!file.type.startsWith('image/')) continue;
      if (file.size > this.maxSizeMB * 1024 * 1024) continue;
      if (this.files.length >= this.maxFiles) break;
      const reader = new FileReader();
      reader.onload = (e: any) => {
        (file as any).preview = e.target.result;
        this.files = [...this.files, file as File & { preview?: string }];
        this.filesChange.emit(this.files);
      };
      reader.readAsDataURL(file);
    }
  }

  removeFile(index: number): void {
    this.files.splice(index, 1);
    this.files = [...this.files];
    this.filesChange.emit(this.files);
  }
} 