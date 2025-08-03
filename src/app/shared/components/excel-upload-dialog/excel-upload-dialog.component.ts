import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { ExcelUploadService, ExcelUploadConfig, ExcelUploadResult } from '../../services/excel-upload.service';

export interface ExcelUploadDialogData {
  title: string;
  description: string;
  config: ExcelUploadConfig;
  templateHeaders: string[];
  templateFilename: string;
  onUpload: (data: any[]) => Promise<boolean>;
}

export interface ExcelUploadDialogResult {
  success: boolean;
  message: string;
  data?: any[];
}

@Component({
  selector: 'app-excel-upload-dialog',
  template: `
    <div class="excel-upload-dialog">
      <div class="dialog-header">
        <h2 mat-dialog-title>{{ data.title }}</h2>
        <p class="description">{{ data.description }}</p>
      </div>

      <mat-dialog-content>
        <div class="upload-section">
          <div class="file-input-container" 
               [class.dragover]="isDragOver"
               (dragover)="onDragOver($event)"
               (dragleave)="onDragLeave($event)"
               (drop)="onDrop($event)">
            
            <input #fileInput type="file" 
                   accept=".xlsx,.xls" 
                   (change)="onFileSelected($event)"
                   style="display: none;">
            
            <div class="upload-area" (click)="fileInput.click()">
              <mat-icon class="upload-icon">cloud_upload</mat-icon>
              <p class="upload-text">
                <strong>Click to upload</strong> or drag and drop
              </p>
              <p class="upload-hint">Excel files (.xlsx, .xls) up to 5MB</p>
            </div>
          </div>

          <div class="template-section">
            <button mat-stroked-button (click)="downloadTemplate()">
              <mat-icon>download</mat-icon>
              Download Template
            </button>
          </div>
        </div>

        <div *ngIf="selectedFile" class="file-info">
          <mat-icon>description</mat-icon>
          <span>{{ selectedFile.name }}</span>
          <span class="file-size">({{ formatFileSize(selectedFile.size) }})</span>
        </div>

        <div *ngIf="isProcessing" class="processing-section">
          <mat-progress-bar mode="indeterminate"></mat-progress-bar>
          <p>Processing Excel file...</p>
        </div>

        <div *ngIf="uploadResult" class="result-section">
          <div class="result-summary">
            <div class="result-item">
              <span class="label">Total Rows:</span>
              <span class="value">{{ uploadResult.totalRows }}</span>
            </div>
            <div class="result-item">
              <span class="label">Valid Rows:</span>
              <span class="value success">{{ uploadResult.validRows }}</span>
            </div>
            <div class="result-item">
              <span class="label">Invalid Rows:</span>
              <span class="value error">{{ uploadResult.invalidRows }}</span>
            </div>
          </div>

          <div *ngIf="uploadResult.errors.length > 0" class="errors-section">
            <h4>Errors Found:</h4>
            <mat-list dense>
              <mat-list-item *ngFor="let error of uploadResult.errors">
                <span class="error-text">{{ error }}</span>
              </mat-list-item>
            </mat-list>
          </div>
        </div>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button (click)="onCancel()">
          Cancel
        </button>
        <button mat-raised-button 
                color="primary" 
                [disabled]="!canUpload()"
                (click)="onUpload()">
          <mat-icon>upload</mat-icon>
          Upload {{ uploadResult?.validRows || 0 }} Items
        </button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .excel-upload-dialog {
      min-width: 500px;
      max-width: 700px;
      padding: 20px;
    }

    .dialog-header {
      margin-bottom: 20px;
    }

    .description {
      color: #666;
      margin: 8px 0 0 0;
    }

    .upload-section {
      margin-bottom: 20px;
    }

    .file-input-container {
      border: 2px dashed #ccc;
      border-radius: 8px;
      padding: 20px;
      text-align: center;
      transition: all 0.3s ease;
      margin-bottom: 16px;
    }

    .file-input-container.dragover {
      border-color: #2196F3;
      background-color: #E3F2FD;
    }

    .upload-area {
      cursor: pointer;
    }

    .upload-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
      color: #666;
      margin-bottom: 8px;
    }

    .upload-text {
      margin: 8px 0;
      font-size: 16px;
    }

    .upload-hint {
      margin: 4px 0;
      color: #666;
      font-size: 14px;
    }

    .template-section {
      text-align: center;
    }

    .file-info {
      display: flex;
      align-items: center;
      gap: 8px;
      padding: 12px;
      background-color: #f5f5f5;
      border-radius: 4px;
      margin-bottom: 16px;
    }

    .file-size {
      color: #666;
      font-size: 12px;
    }

    .processing-section {
      text-align: center;
      padding: 20px;
    }

    .result-section {
      margin-top: 20px;
    }

    .result-summary {
      display: grid;
      grid-template-columns: repeat(3, 1fr);
      gap: 16px;
      margin-bottom: 16px;
    }

    .result-item {
      text-align: center;
      padding: 12px;
      background-color: #f5f5f5;
      border-radius: 4px;
    }

    .result-item .label {
      display: block;
      font-size: 12px;
      color: #666;
      margin-bottom: 4px;
    }

    .result-item .value {
      display: block;
      font-size: 18px;
      font-weight: bold;
    }

    .result-item .value.success {
      color: #4CAF50;
    }

    .result-item .value.error {
      color: #f44336;
    }

    .errors-section {
      margin-top: 16px;
    }

    .errors-section h4 {
      margin: 0 0 8px 0;
      color: #f44336;
    }

    .error-text {
      color: #f44336;
      font-size: 14px;
    }

    mat-dialog-actions {
      padding: 16px 0 0 0;
    }
  `],
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatListModule,
    MatCardModule,
    MatSnackBarModule
  ]
})
export class ExcelUploadDialogComponent implements OnInit {
  selectedFile: File | null = null;
  isDragOver = false;
  isProcessing = false;
  uploadResult: ExcelUploadResult<any> | null = null;

  constructor(
    public dialogRef: MatDialogRef<ExcelUploadDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ExcelUploadDialogData,
    private excelUploadService: ExcelUploadService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {}

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = false;
    
    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      this.handleFile(files[0]);
    }
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.handleFile(input.files[0]);
    }
  }

  private handleFile(file: File): void {
    // Validate file type
    if (!this.excelUploadService.validateFileType(file)) {
      this.snackBar.open('Please select a valid Excel file (.xlsx, .xls)', 'Close', { duration: 3000 });
      return;
    }

    // Validate file size
    if (!this.excelUploadService.validateFileSize(file)) {
      this.snackBar.open('File size must be less than 5MB', 'Close', { duration: 3000 });
      return;
    }

    this.selectedFile = file;
    this.processFile();
  }

  private async processFile(): Promise<void> {
    if (!this.selectedFile) return;

    this.isProcessing = true;
    this.uploadResult = null;

    try {
      this.uploadResult = await this.excelUploadService.parseExcelFile(this.selectedFile, this.data.config);
      console.log('Excel upload result:', this.uploadResult);
    } catch (error) {
      console.error('Error processing Excel file:', error);
      this.snackBar.open('Error processing Excel file: ' + error, 'Close', { duration: 4000 });
    } finally {
      this.isProcessing = false;
    }
  }

  downloadTemplate(): void {
    this.excelUploadService.downloadTemplate(
      this.data.templateHeaders,
      this.data.templateFilename
    );
  }

  canUpload(): boolean {
    return this.uploadResult !== null && this.uploadResult.validRows > 0;
  }

  async onUpload(): Promise<void> {
    if (!this.uploadResult || !this.canUpload()) return;

    try {
      const success = await this.data.onUpload(this.uploadResult.data);
      
      if (success) {
        this.dialogRef.close({
          success: true,
          message: `Successfully uploaded ${this.uploadResult.validRows} items`,
          data: this.uploadResult.data
        });
      } else {
        this.snackBar.open('Failed to upload items', 'Close', { duration: 3000 });
      }
    } catch (error) {
      console.error('Error uploading data:', error);
      this.snackBar.open('Error uploading data: ' + error, 'Close', { duration: 4000 });
    }
  }

  onCancel(): void {
    this.dialogRef.close({
      success: false,
      message: 'Upload cancelled'
    });
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }
} 