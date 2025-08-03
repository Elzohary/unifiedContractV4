import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { finalize, catchError, takeUntil } from 'rxjs/operators';
import { EMPTY, Subject } from 'rxjs';
import { WorkOrderItemService } from '../../services/work-order-item.service';
import { Iitem } from '../../models/work-order-item.model';
import { CreateMasterItemDialogComponent } from './create-master-item-dialog.component';
import { AssignWorkOrderItemDialogComponent } from '../work-order-item-dialog/assign-work-order-item-dialog.component';
import { NgCardComponent } from '../../../../shared/components/ng-card/ng-card.component';
import { ExcelUploadDialogComponent, ExcelUploadDialogData } from '../../../../shared/components/excel-upload-dialog/excel-upload-dialog.component';
import { ExcelUploadService, ExcelUploadConfig } from '../../../../shared/services/excel-upload.service';
import { AuthService } from '../../../../shared/services/auth.service';

@Component({
  selector: 'app-work-order-items-list',
  templateUrl: './work-order-items-list.component.html',
  styleUrls: ['./work-order-items-list.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatDialogModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    NgCardComponent,
    ExcelUploadDialogComponent
  ]
})
export class WorkOrderItemsListComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() workOrderId!: string;
  @Output() itemsUpdated = new EventEmitter<Iitem[]>();

  displayedColumns: string[] = [
    'itemNumber',
    'shortDescription',
    'longDescription',
    'UOM',
    'currency',
    'paymentType',
    'managementArea',
    'actions'
  ];

  dataSource: MatTableDataSource<Iitem>;
  isLoading = false;
  private destroy$ = new Subject<void>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private workOrderItemService: WorkOrderItemService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private excelUploadService: ExcelUploadService,
    private authService: AuthService
  ) {
    this.dataSource = new MatTableDataSource();
  }

  ngOnInit(): void {
    // Subscribe to the items$ observable from the service
    this.loadItems();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadItems(): void {
    this.isLoading = true;
    let itemsObservable;
    if (this.workOrderId) {
      itemsObservable = this.workOrderItemService.getItems(this.workOrderId);
    } else {
      itemsObservable = this.workOrderItemService.getAllItems();
    }
    itemsObservable
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => this.isLoading = false),
        catchError(error => {
          this.showErrorMessage('Failed to load items. Please try again.');
          console.error('Error loading items:', error);
          return EMPTY;
        })
      )
      .subscribe(items => {
        this.dataSource.data = items;
        this.isLoading = false;
        // Ensure table refreshes
        this.dataSource._updateChangeSubscription();
      });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  openAddDialog(): void {
    const dialogRef = this.dialog.open(CreateMasterItemDialogComponent, {
      width: '600px',
      maxWidth: '100vw',
      maxHeight: '100vh',
      panelClass: 'full-screen-dialog',
      data: {
        title: 'Create New Master Item'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Optimistic UI update - add temporary item with loading state
        const tempId = 'temp-' + new Date().getTime();
        const tempItem = { ...result, id: tempId };

        // Add to current data and update view
        const currentData = [...this.dataSource.data];
        currentData.unshift(tempItem);
        this.dataSource.data = currentData;

        // Make actual API call
        this.workOrderItemService.createItem(result)
          .pipe(
            catchError(error => {
              // Remove temporary item on error
              this.removeItemById(tempId);
              this.showErrorMessage('Failed to create item. Please try again.');
              console.error('Error creating item:', error);
              return EMPTY;
            })
          )
          .subscribe(createdItem => {
            // Replace temp item with actual item
            this.removeItemById(tempId);
            this.dataSource.data = [createdItem, ...this.dataSource.data];
            this.showSuccessMessage('Item created successfully');
          });
      }
    });
  }

  openEditDialog(item: Iitem): void {
    // Remove any usage of WorkOrderItemDialogComponent (should only use CreateMasterItemDialogComponent now)
  }

  openAssignDialog(): void {
    const dialogRef = this.dialog.open(AssignWorkOrderItemDialogComponent, {
      width: '600px',
      maxWidth: '100vw',
      maxHeight: '100vh',
      panelClass: 'full-screen-dialog',
      data: {
        title: 'Assign Work Order Item',
        workOrderId: this.workOrderId,
        assignedItemIds: this.dataSource.data.map(item => item.id)
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Call backend to assign the item
        this.workOrderItemService.assignItemToWorkOrder(this.workOrderId, result)
          .pipe(
            catchError(error => {
              this.showErrorMessage('Failed to assign item. Please try again.');
              console.error('Error assigning item:', error);
              return EMPTY;
            })
          )
          .subscribe(() => {
            this.showSuccessMessage('Item assigned successfully');
            this.loadItems();
          });
      }
    });
  }

  // Confirm delete with the user
  confirmDelete(item: Iitem): void {
    if (confirm(`Are you sure you want to delete item "${item.shortDescription}"?`)) {
      this.deleteItem(item);
    }
  }

  // Delete the item and update the table
  deleteItem(item: Iitem): void {
    // First remove from UI immediately for better UX
    const currentData = this.dataSource.data.filter(i => i.id !== item.id);
    this.dataSource.data = currentData;

    // Then actually delete from the backend
    this.workOrderItemService.deleteItem(item.id)
      .pipe(
        catchError(error => {
          // If deletion fails, reload the original data
          this.loadItems();
          this.showErrorMessage('Failed to delete item. Please try again.');
          console.error('Error deleting item:', error);
          return EMPTY;
        })
      )
      .subscribe(() => {
        this.showSuccessMessage('Item deleted successfully');
      });
  }

  private removeItemById(id: string): void {
    const currentData = this.dataSource.data.filter(item => item.id !== id);
    this.dataSource.data = currentData;
  }

  private showSuccessMessage(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: 'success-snackbar'
    });
  }

  private showErrorMessage(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 5000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: 'error-snackbar'
    });
  }

  // Method to emit updated items
  updateItems(items: Iitem[]): void {
    this.itemsUpdated.emit(items);
  }

  openExcelUploadDialog(): void {
    const config: ExcelUploadConfig = {
      requiredColumns: [
        'Item',
        'Short Description'
      ],
      dataTransformer: (row: any) => {
        return {
          itemNumber: row['Item']?.toString().trim() || '',
          lineType: row['Line type']?.toString().trim() || 'Description',
          shortDescription: row['Short Description']?.toString().trim() || '',
          longDescription: row['Long Description']?.toString().trim() || '',
          UOM: row['UOM']?.toString().trim() || 'Piece', // Required field, must have default
          currency: row['Currency']?.toString().trim() || 'SAR',
          unitPrice: this.parseUnitPrice(row['ادارة كهرباء الأحساء']) || 0,
          paymentType: row['Payment Type']?.toString().trim() || 'Fixed Price',
          managementArea: 'ادارة كهرباء الأحساء',
          clientId: this.authService.getClientIdFromToken() || '00000000-0000-0000-0000-000000000000',
          isActive: true
        };
      },
      validator: (row: any) => {
        const errors: string[] = [];
        
        if (!row['Item'] || row['Item'].toString().trim() === '') {
          errors.push('Item number is required');
        }
        
        if (!row['Short Description'] || row['Short Description'].toString().trim() === '') {
          errors.push('Short description is required');
        }
        

        
        // Only validate unit price if it's provided
        if (row['ادارة كهرباء الأحساء'] && row['ادارة كهرباء الأحساء'].toString().trim() !== '') {
          const unitPrice = this.parseUnitPrice(row['ادارة كهرباء الأحساء']);
          if (isNaN(unitPrice) || unitPrice < 0) {
            errors.push('Unit price must be a valid positive number');
          }
        }
        
        return {
          isValid: errors.length === 0,
          errors
        };
      }
    };

    const dialogData: ExcelUploadDialogData = {
      title: 'Upload Items from Excel',
      description: 'Upload multiple items from an Excel file. The file should contain the following columns: Item, Line type, Short Description, Long Description, UOM, Currency, Payment Type, and ادارة كهرباء الأحساء (unit rates).',
      config,
      templateHeaders: [
        'Item',
        'Line type',
        'Short Description',
        'Long Description',
        'UOM',
        'Currency',
        'Payment Type',
        'ادارة كهرباء الأحساء'
      ],
      templateFilename: 'items_template.xlsx',
      onUpload: async (data: any[]) => {
        try {
          console.log('Uploading items data:', data);
          
          // Create items one by one
          const results = await Promise.all(
            data.map(itemData => 
              this.workOrderItemService.createItem(itemData).toPromise()
            )
          );
          
          // Check if all items were created successfully
          const successCount = results.filter((result: any) => result !== undefined).length;
          console.log(`Successfully created ${successCount} out of ${data.length} items`);
          
          // Reload items list
          this.loadItems();
          
          return successCount === data.length;
        } catch (error) {
          console.error('Error uploading items:', error);
          return false;
        }
      }
    };

    const dialogRef = this.dialog.open(ExcelUploadDialogComponent, {
      width: '700px',
      data: dialogData,
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result?.success) {
        this.snackBar.open(result.message, 'Close', { duration: 3000 });
      }
    });
  }

  private parseUnitPrice(value: any): number {
    if (typeof value === 'number') {
      return value;
    }
    
    if (typeof value === 'string') {
      // Remove any non-numeric characters except decimal point
      const cleaned = value.replace(/[^\d.]/g, '');
      const parsed = parseFloat(cleaned);
      return isNaN(parsed) ? 0 : parsed;
    }
    
    return 0;
  }
}
