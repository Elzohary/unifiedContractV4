import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { WorkOrderDetailsRefactoredComponent } from './work-order-details-refactored.component';
import { WoSiteReportTabComponent } from './components/wo-site-report-tab/wo-site-report-tab.component';
import { WoOverviewTabComponent } from './components/wo-overview-tab/wo-overview-tab.component';
import { of } from 'rxjs';
import { WorkOrder } from '../../models/work-order.model';
import { WorkOrderDetailsViewModel } from '../../viewModels/work-order-details.viewmodel';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { WorkOrderRemarksViewModel } from '../../viewModels/work-order-remarks.viewmodel';
import { WorkOrderMaterialsViewModel } from '../../viewModels/work-order-materials.viewmodel';
import { ActivatedRoute } from '@angular/router';

// Mock ViewModel
class WorkOrderDetailsViewModelMock {
  workOrder$ = of({
    id: 'wo1',
    details: {
      workOrderNumber: 'WO-001',
      internalOrderNumber: 'INT-001',
      title: 'Test Work Order',
      client: 'Test Client',
      location: 'Test Location',
      status: 'pending',
      priority: 'medium',
      category: 'General',
      completionPercentage: 0,
      receivedDate: new Date(),
      createdDate: new Date(),
      createdBy: 'user1'
    },
    items: [
      { id: 'item1', itemDetail: { shortDescription: 'Work Item 1' }, actualQuantity: 0 }
    ],
    remarks: [],
    issues: [],
    materials: [],
    siteReports: []
  });
  loading$ = of(false);
  error$ = of(null);
  activeTab$ = of('overview');
  activityLogs$ = of([]);
  tasks$ = of([]);
  updateWorkOrder = jasmine.createSpy('updateWorkOrder').and.returnValue(of({ id: 'wo1' }));
  loadWorkOrderDetails = jasmine.createSpy('loadWorkOrderDetails');
  destroy = jasmine.createSpy('destroy');
}
class WorkOrderRemarksViewModelMock {
  destroy = jasmine.createSpy('destroy');
}
class WorkOrderMaterialsViewModelMock {
  destroy = jasmine.createSpy('destroy');
}

describe('WorkOrderDetailsRefactoredComponent', () => {
  let component: WorkOrderDetailsRefactoredComponent;
  let fixture: ComponentFixture<WorkOrderDetailsRefactoredComponent>;
  let viewModel: WorkOrderDetailsViewModelMock;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkOrderDetailsRefactoredComponent, WoSiteReportTabComponent, WoOverviewTabComponent, HttpClientTestingModule],
      providers: [
        { provide: WorkOrderDetailsViewModel, useClass: WorkOrderDetailsViewModelMock },
        { provide: ActivatedRoute, useValue: {
          paramMap: of({ get: () => 'wo1' }),
          fragment: of(null)
        } },
        { provide: WorkOrderRemarksViewModel, useClass: WorkOrderRemarksViewModelMock },
        { provide: WorkOrderMaterialsViewModel, useClass: WorkOrderMaterialsViewModelMock }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkOrderDetailsRefactoredComponent);
    component = fixture.componentInstance;
    viewModel = TestBed.inject(WorkOrderDetailsViewModel) as any;
    fixture.detectChanges();
  });

  it('should update actualQuantity in overview tab after site report is added', fakeAsync(() => {
    // Simulate event from site report tab
    const formValue = {
      workDone: 'item1',
      actualQuantity: 5,
      foremanName: 'Test Foreman',
      date: new Date(),
      materialsUsed: [],
      photos: [],
      notes: 'Test note'
    };
    component.onSiteReportAdded(formValue);
    tick();
    fixture.detectChanges();
    // The view model's updateWorkOrder should have been called
    expect(viewModel.updateWorkOrder).toHaveBeenCalled();
    // The view model's loadWorkOrderDetails should have been called with the correct id
    expect(viewModel.loadWorkOrderDetails).toHaveBeenCalledWith('wo1');
  }));
}); 