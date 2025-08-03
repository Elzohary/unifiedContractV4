import { Injectable } from '@angular/core';
import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';

export interface ExcelUploadConfig {
  sheetName?: string;
  requiredColumns: string[];
  optionalColumns?: string[];
  dataTransformer?: (row: any) => any;
  validator?: (row: any) => { isValid: boolean; errors: string[] };
}

export interface ExcelUploadResult<T> {
  success: boolean;
  data: T[];
  errors: string[];
  totalRows: number;
  validRows: number;
  invalidRows: number;
}

@Injectable({
  providedIn: 'root'
})
export class ExcelUploadService {

  constructor() { }

  /**
   * Parse Excel file and return structured data
   */
  parseExcelFile<T>(file: File, config: ExcelUploadConfig): Promise<ExcelUploadResult<T>> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      
      reader.onload = (e: any) => {
        try {
          const data = new Uint8Array(e.target.result);
          const workbook = XLSX.read(data, { type: 'array' });
          const sheetName = config.sheetName || workbook.SheetNames[0];
          const worksheet = workbook.Sheets[sheetName];
          
          if (!worksheet) {
            reject(new Error(`Sheet '${sheetName}' not found in Excel file`));
            return;
          }

          const json: any[] = XLSX.utils.sheet_to_json(worksheet, { defval: '' });
          console.log('[Excel Upload] Parsed rows:', json);

          const result = this.processExcelData<T>(json, config);
          resolve(result);
        } catch (error) {
          reject(error);
        }
      };

      reader.onerror = () => {
        reject(new Error('Failed to read the file'));
      };

      reader.readAsArrayBuffer(file);
    });
  }

  /**
   * Process Excel data with validation and transformation
   */
  private processExcelData<T>(rows: any[], config: ExcelUploadConfig): ExcelUploadResult<T> {
    const result: ExcelUploadResult<T> = {
      success: true,
      data: [],
      errors: [],
      totalRows: rows.length,
      validRows: 0,
      invalidRows: 0
    };

    for (let i = 0; i < rows.length; i++) {
      const row = rows[i];
      const rowNumber = i + 2; // +2 because Excel is 1-based and we have headers

      try {
        // Validate required columns
        const missingColumns = config.requiredColumns.filter(col => 
          !row[col] || row[col].toString().trim() === ''
        );

        if (missingColumns.length > 0) {
          result.errors.push(`Row ${rowNumber}: Missing required columns: ${missingColumns.join(', ')}`);
          result.invalidRows++;
          continue;
        }

        // Custom validation if provided
        if (config.validator) {
          const validation = config.validator(row);
          if (!validation.isValid) {
            result.errors.push(`Row ${rowNumber}: ${validation.errors.join(', ')}`);
            result.invalidRows++;
            continue;
          }
        }

        // Transform data if transformer provided
        let transformedRow = row;
        if (config.dataTransformer) {
          transformedRow = config.dataTransformer(row);
        }

        result.data.push(transformedRow as T);
        result.validRows++;

      } catch (error) {
        result.errors.push(`Row ${rowNumber}: Error processing row - ${error}`);
        result.invalidRows++;
      }
    }

    result.success = result.invalidRows === 0;
    return result;
  }

  /**
   * Download Excel template
   */
  downloadTemplate(headers: string[], filename: string = 'template.xlsx'): void {
    const worksheet = XLSX.utils.aoa_to_sheet([headers]);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Template');
    
    const excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    saveAs(blob, filename);
  }

  /**
   * Download data as Excel file
   */
  downloadExcel<T>(data: T[], headers: string[], filename: string = 'export.xlsx'): void {
    // Convert data to array format
    const rows = data.map(item => {
      const row: any = {};
      headers.forEach(header => {
        row[header] = (item as any)[header] || '';
      });
      return row;
    });

    const worksheet = XLSX.utils.json_to_sheet(rows);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Data');
    
    const excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    saveAs(blob, filename);
  }

  /**
   * Validate file type
   */
  validateFileType(file: File): boolean {
    const allowedTypes = [
      'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      'application/vnd.ms-excel',
      'application/vnd.ms-excel.sheet.macroEnabled.12'
    ];
    return allowedTypes.includes(file.type);
  }

  /**
   * Validate file size (default 5MB)
   */
  validateFileSize(file: File, maxSizeMB: number = 5): boolean {
    const maxSizeBytes = maxSizeMB * 1024 * 1024;
    return file.size <= maxSizeBytes;
  }
} 