# Excel Upload Functionality

## Overview

This application now includes a reusable Excel upload system that allows users to upload multiple items at once using Excel files. The system is designed to be modular and can be easily extended for other data types.

## Features

### ✅ **Reusable Excel Upload Service**
- **File**: `src/app/shared/services/excel-upload.service.ts`
- **Purpose**: Generic Excel parsing and validation service
- **Features**:
  - File type validation (.xlsx, .xls)
  - File size validation (default 5MB)
  - Custom data transformation
  - Custom validation rules
  - Error reporting with row numbers
  - Template download functionality

### ✅ **Reusable Excel Upload Dialog**
- **File**: `src/app/shared/components/excel-upload-dialog/excel-upload-dialog.component.ts`
- **Purpose**: User-friendly dialog for Excel uploads
- **Features**:
  - Drag and drop file upload
  - File validation
  - Progress indication
  - Error display
  - Template download
  - Success/failure feedback

### ✅ **Items Excel Upload Implementation**
- **Location**: Items list page (`/work-order-sections/items-list`)
- **Button**: "Upload Excel" button next to "Add Item"
- **Columns**: 
  - Item (required)
  - Line type
  - Short Description (required)
  - Long Description
  - UOM (required)
  - Currency
  - Payment Type
  - ادارة كهرباء الأحساء (unit rates - required)

## How to Use

### 1. **Download Template**
1. Navigate to `/work-order-sections/items-list`
2. Click the "Upload Excel" button
3. Click "Download Template" in the dialog
4. The template will be downloaded as `items_template.xlsx`

### 2. **Prepare Excel File**
Fill in the template with your data:

| Column | Description | Required | Example |
|--------|-------------|----------|---------|
| Item | Item number/code | ✅ | "001" |
| Line type | Item line type | ❌ | "Description" |
| Short Description | Brief item description | ✅ | "Electrical Cable" |
| Long Description | Detailed description | ❌ | "Copper electrical cable 2.5mm" |
| UOM | Unit of measure | ❌ | "Meter" |
| Currency | Currency code | ❌ | "SAR" |
| Payment Type | Payment method | ❌ | "Fixed Price" |
| ادارة كهرباء الأحساء | Unit price | ❌ | "15.50" |

### 3. **Upload File**
1. Click "Upload Excel" button
2. Drag and drop your Excel file or click to browse
3. The system will validate the file and show results
4. Review any errors if present
5. Click "Upload X Items" to proceed

## Validation Rules

### **Required Fields**
- Item number
- Short description

### **Optional Fields with Defaults**
- UOM: Defaults to "Piece"
- Currency: Defaults to "SAR"
- Payment Type: Defaults to "Fixed Price"
- Unit price: Defaults to 0 (only validated if provided)

### **Data Validation**
- Unit price must be a valid positive number (if provided)
- All required fields must not be empty
- File must be Excel format (.xlsx, .xls)
- File size must be under 5MB

## Error Handling

The system provides detailed error reporting:
- **Row-specific errors**: Shows exactly which row has issues
- **Field-specific errors**: Identifies which fields are missing or invalid
- **File validation**: Checks file type and size before processing
- **Upload feedback**: Shows success/failure count

## Extending for Other Data Types

To add Excel upload for other data types (e.g., employees, materials):

### 1. **Create Configuration**
```typescript
const config: ExcelUploadConfig = {
  requiredColumns: ['Column1', 'Column2'],
  dataTransformer: (row: any) => {
    return {
      // Transform Excel row to your data model
    };
  },
  validator: (row: any) => {
    // Add validation logic
    return { isValid: true, errors: [] };
  }
};
```

### 2. **Create Dialog Data**
```typescript
const dialogData: ExcelUploadDialogData = {
  title: 'Upload Your Data',
  description: 'Description of what to upload',
  config,
  templateHeaders: ['Column1', 'Column2'],
  templateFilename: 'your_template.xlsx',
  onUpload: async (data: any[]) => {
    // Handle the upload
    return true; // or false if failed
  }
};
```

### 3. **Open Dialog**
```typescript
const dialogRef = this.dialog.open(ExcelUploadDialogComponent, {
  width: '700px',
  data: dialogData
});
```

## Technical Details

### **Dependencies**
- `xlsx`: Excel file parsing
- `file-saver`: File download functionality
- Angular Material: UI components

### **File Structure**
```
src/app/shared/
├── services/
│   └── excel-upload.service.ts          # Core Excel processing
└── components/
    └── excel-upload-dialog/
        └── excel-upload-dialog.component.ts  # Upload UI
```

### **API Integration**
The Excel upload integrates with existing backend APIs:
- **Items**: Uses `/api/work-orders/master-items` endpoint
- **Error Handling**: Graceful handling of API failures
- **Batch Processing**: Creates items one by one with proper error handling

## Example Excel Template

Here's what the template looks like:

| Item | Line type | Short Description | Long Description | UOM | Currency | Payment Type | ادارة كهرباء الأحساء |
|------|-----------|-------------------|------------------|-----|----------|--------------|---------------------|
| 001 | Description | Electrical Cable | Copper cable 2.5mm | Meter | SAR | Fixed Price | 15.50 |
| 002 | Description | Circuit Breaker | 32A circuit breaker | Piece | SAR | Fixed Price | 45.00 |

## Troubleshooting

### **Common Issues**
1. **"File type not supported"**: Ensure file is .xlsx or .xls
2. **"File too large"**: Reduce file size to under 5MB
3. **"Missing required columns"**: Check column headers match template (only Item and Short Description are required)
4. **"Invalid unit price"**: Ensure price is a positive number (only if provided)

### **Debug Information**
- Check browser console for detailed error logs
- Review validation errors in the upload dialog
- Verify Excel file format and data types

## Future Enhancements

Potential improvements for the Excel upload system:
- **Batch API endpoints**: Upload all items in one API call
- **Progress tracking**: Show upload progress for large files
- **Data preview**: Show data before upload
- **Column mapping**: Allow users to map Excel columns to fields
- **Template customization**: Allow different templates for different use cases 