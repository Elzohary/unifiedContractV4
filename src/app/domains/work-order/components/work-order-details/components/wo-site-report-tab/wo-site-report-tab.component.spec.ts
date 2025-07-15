import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { WoSiteReportTabComponent } from './wo-site-report-tab.component';
import { SiteReportFormComponent } from './site-report-form.component';
import { MatDialog } from '@angular/material/dialog';
import { of } from 'rxjs';
import { WorkOrder } from '../../../../models/work-order.model';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { WorkOrderService } from '../../../../services/work-order.service';

// Mock MatDialog
class MatDialogMock {
  open() {
    return {
      afterClosed: () => of({
        date: new Date(),
        foremanName: 'Test Foreman',
        workDone: 'item1',
        actualQuantity: 5,
        notes: 'Test note',
        materialsUsed: [],
        photos: []
      })
    };
  }
}

// Mock WorkOrderService
class WorkOrderServiceMock {}

describe('WoSiteReportTabComponent', () => {
  let component: WoSiteReportTabComponent;
  let fixture: ComponentFixture<WoSiteReportTabComponent>;
  let workOrder: WorkOrder;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WoSiteReportTabComponent, SiteReportFormComponent, HttpClientTestingModule],
      providers: [
        { provide: MatDialog, useClass: MatDialogMock },
        { provide: WorkOrderService, useClass: WorkOrderServiceMock }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WoSiteReportTabComponent);
    component = fixture.componentInstance;
    workOrder = {
      id: 'wo1',
      items: [
        { id: 'item1', itemDetail: { shortDescription: 'Work Item 1' }, actualQuantity: 0 }
      ],
      materials: [],
      siteReports: []
    } as any;
    component.workOrder = workOrder;
    fixture.detectChanges();
  });

  it('should emit updated event with new site report and update actualQuantity', fakeAsync(() => {
    spyOn(component.updated, 'emit');
    component.openSampleFormDialog();
    tick();
    fixture.detectChanges();
    expect(component.updated.emit).toHaveBeenCalled();
    const emittedValue = (component.updated.emit as jasmine.Spy).calls.mostRecent().args[0];
    expect(emittedValue.workDone).toBe('item1');
    expect(emittedValue.actualQuantity).toBe(5);
  }));
}); 