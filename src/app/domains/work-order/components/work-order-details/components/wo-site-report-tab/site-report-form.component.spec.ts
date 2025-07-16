import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { SiteReportFormComponent, WorkDoneLabelPipe, MaterialNamePipe } from './site-report-form.component';
import { SiteReportStatus } from '../../../../models/work-order.model';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { UserService } from '../../../../../../shared/services/user.service';

describe('SiteReportFormComponent', () => {
  let component: SiteReportFormComponent;
  let fixture: ComponentFixture<SiteReportFormComponent>;
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<SiteReportFormComponent>>;

  const mockWorkOrder = {
    id: 'wo1',
    items: [
      { id: 'item1', itemDetail: { shortDescription: 'Excavation' } },
      { id: 'item2', itemDetail: { shortDescription: 'Concrete Pouring' } }
    ],
    materials: [
      { id: 'mat1', materialType: 'purchasable', purchasableMaterial: { name: 'Cement', status: 'in-use' } },
      { id: 'mat2', materialType: 'receivable', receivableMaterial: { name: 'Sand', status: 'received' } }
    ]
  };

  beforeEach(async () => {
    dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);
    await TestBed.configureTestingModule({
      imports: [
        SiteReportFormComponent,
        ReactiveFormsModule,
        NoopAnimationsModule,
        WorkDoneLabelPipe,
        MaterialNamePipe,
        HttpClientTestingModule
      ],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: { workOrder: mockWorkOrder } },
        { provide: UserService, useValue: { getCurrentUserName: () => 'Test Foreman' } }
      ]
    }).compileComponents();
    fixture = TestBed.createComponent(SiteReportFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should render all steps', () => {
    // Check for the presence of step labels in the DOM text
    const html = fixture.nativeElement.textContent;
    expect(html).toContain('Site Safety Arrangements');
    expect(html).toContain('Work Done & Materials');
    expect(html).toContain('Site Housekeeping');
    expect(html).toContain('Review & Submit');
  });

  it('should require at least one safety photo to proceed', () => {
    component.onSafetyPhotosChange([]); // Simulate no files selected
    fixture.detectChanges();
    expect(component.step1FormGroup.get('safetyPhotos')?.valid).toBeFalse();
    component.onSafetyPhotosChange([new File([''], 'photo1.jpg', { type: 'image/jpeg' })]);
    fixture.detectChanges();
    expect(component.step1FormGroup.get('safetyPhotos')?.valid).toBeTrue();
  });

  it('should require at least one housekeeping photo to submit', () => {
    component.onHousekeepingPhotosChange([]); // Simulate no files selected
    fixture.detectChanges();
    expect(component.step3FormGroup.get('housekeepingPhotos')?.valid).toBeFalse();
    component.onHousekeepingPhotosChange([new File([''], 'house1.jpg', { type: 'image/jpeg' })]);
    fixture.detectChanges();
    expect(component.step3FormGroup.get('housekeepingPhotos')?.valid).toBeTrue();
  });

  it('should validate required work done field', () => {
    component.step2FormGroup.get('workDone')?.setValue('');
    fixture.detectChanges();
    expect(component.step2FormGroup.get('workDone')?.valid).toBeFalse();
  });

  it('should show review step with summary', () => {
    // Fill all required fields
    component.onSafetyPhotosChange([new File([''], 'photo1.jpg', { type: 'image/jpeg' })]);
    component.step2FormGroup.get('workDone')?.setValue('item1');
    component.onHousekeepingPhotosChange([new File([''], 'house1.jpg', { type: 'image/jpeg' })]);
    fixture.detectChanges();
    expect(component.form.valid).toBeTrue();
    expect(component.form.get('foremanName')?.value).toBeDefined();
    expect(component.step2FormGroup.get('workDone')?.value).toBe('item1');
  });

  it('should emit correct data on submit', () => {
    component.onSafetyPhotosChange([new File([''], 'photo1.jpg', { type: 'image/jpeg' })]);
    component.step2FormGroup.get('workDone')?.setValue('item1');
    component.onHousekeepingPhotosChange([new File([''], 'house1.jpg', { type: 'image/jpeg' })]);
    fixture.detectChanges();
    component.saveReport(SiteReportStatus.Closed);
    expect(dialogRefSpy.close).toHaveBeenCalled();
    const result = dialogRefSpy.close.calls.mostRecent().args[0];
    expect(result.report.status).toBe(SiteReportStatus.Closed);
    expect(result.report.workDone).toBe('item1');
  });

  it('should handle no materials gracefully', () => {
    component.materialOptions = [];
    component.materialsUsed.clear();
    fixture.detectChanges();
    expect(component.materialOptions.length).toBe(0);
    expect(component.materialsUsed.length).toBe(0);
  });

  it('should handle no work done options gracefully', () => {
    component.workDoneOptions = [];
    component.step2FormGroup.get('workDone')?.setValue('');
    fixture.detectChanges();
    expect(component.workDoneOptions.length).toBe(0);
  });

  // Remove add/remove photo tests for safety/housekeeping photos (obsolete)
  it('should allow adding and removing materials', () => {
    component.materialsUsed.clear();
    expect(component.materialsUsed.length).toBe(0);
    // Add a public getFormBuilder() method to the component for test use
    const fb = (component as any).getFormBuilder ? (component as any).getFormBuilder() : undefined;
    if (fb) {
      component.materialsUsed.push(fb.group({
        materialId: ['mat1', []],
        quantity: [1, []]
      }));
    } else {
      // fallback: push a plain object (should fail validation, but test structure)
      component.materialsUsed.push({ materialId: 'mat1', quantity: 1 } as any);
    }
    expect(component.materialsUsed.length).toBe(1);
    component.materialsUsed.removeAt(0);
    expect(component.materialsUsed.length).toBe(0);
  });
}); 