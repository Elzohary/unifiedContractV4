import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WorkOrderListComponent } from './domains/work-order/components/work-order-list/work-order-list.component';
import { WorkOrderDetailsComponent } from './domains/work-order/components/work-order-details/work-order-details.component';
import { WorkOrderFormComponent } from './domains/work-order/components/work-order-form/work-order-form.component';
import { OverviewComponent } from './domains/work-order/components/overview/overview.component';
import { UnderConstructionComponent } from './shared/components/under-construction/under-construction.component';
import { AllRemarksComponent } from './features/dashboards/dashboard-management/components/all-remarks/all-remarks.component';
import { ActivityLogPageComponent } from './features/dashboards/dashboard-management/components/activity-log-page/activity-log-page.component';
import { EquipmentDashboardComponent } from './modules/resources/components/equipment-dashboard/equipment-dashboard.component';
import { LoginComponent } from './auth/login/login.component';
import { UserListComponent } from './users/user-list/user-list.component';
import { WorkOrderItemsListComponent } from './domains/work-order/components/work-order-items-list/work-order-items-list.component';
import { MaterialsManagementComponent } from './domains/materials/components/materials-management/materials-management.component';
import { WorkOrderDetailsRefactoredComponent } from './domains/work-order/components/work-order-details/work-order-details-refactored.component';
import { MaterialInventoryDashboardComponent } from './domains/materials/components/material-inventory-dashboard/material-inventory-dashboard.component';
import { AuthGuard } from './shared/guards/auth.guard';

export const routes: Routes = [
  // Public routes (no auth required)
  { path: 'login', component: LoginComponent },

  // Default route - redirect to dashboard
  {
    path: '',
    redirectTo: 'dashboard/overview',
    pathMatch: 'full'
  },

  // Protected routes with canActivate
  {
    path: 'dashboard',
    redirectTo: 'dashboard/overview',
    pathMatch: 'full'
  },
  {
    path: 'dashboard/overview',
    component: OverviewComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'dashboard/analytics',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: { title: 'Analytics Dashboard', message: 'The analytics dashboard is currently under development.' }
  },
  {
    path: 'dashboard/projects',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: { title: 'Projects Dashboard', message: 'The projects dashboard is currently under development.' }
  },

  // Work Order routes - protected
  {
    path: 'work-orders',
    component: WorkOrderListComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'work-orders/list',
    component: WorkOrderListComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'work-orders/new',
    component: WorkOrderFormComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'work-orders/edit/:id',
    component: WorkOrderFormComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'work-orders/details/:id',
    component: WorkOrderDetailsRefactoredComponent,
    canActivate: [AuthGuard]
  },

  // Work order sections - protected
  {
    path: 'work-order-sections/remarks',
    component: AllRemarksComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'work-order-sections/activity-log',
    component: ActivityLogPageComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'work-order-sections/issues',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: {
      title: 'Work Order Issues',
      message: 'The issues section is currently under development.',
      category: 'work-orders'
    }
  },
  {
    path: 'work-order-sections/actions',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: {
      title: 'Work Order Actions',
      message: 'The actions needed section is currently under development.',
      category: 'work-orders'
    }
  },
  {
    path: 'work-order-sections/materials',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: {
      title: 'Work Order Materials',
      message: 'The materials section is currently under development.',
      category: 'work-orders'
    }
  },
  {
    path: 'work-order-sections/photos',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: {
      title: 'Work Order Photos',
      message: 'The photos section is currently under development.',
      category: 'work-orders'
    }
  },
  {
    path: 'work-order-sections/forms',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: {
      title: 'Work Order Forms',
      message: 'The forms section is currently under development.',
      category: 'work-orders'
    }
  },
  {
    path: 'work-order-sections/expenses',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: {
      title: 'Work Order Expenses',
      message: 'The expenses section is currently under development.',
      category: 'work-orders'
    }
  },
  {
    path: 'work-order-sections/invoices',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: {
      title: 'Work Order Invoices',
      message: 'The invoices section is currently under development.',
      category: 'work-orders'
    }
  },
  {
    path: 'work-order-sections/items-list',
    component: WorkOrderItemsListComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },

  // Resources routes - protected
  {
    path: 'resources',
    redirectTo: 'resources/manpower',
    pathMatch: 'full'
  },
  {
    path: 'resources/manpower',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: { title: 'Manpower Management', message: 'The manpower management page is currently under development.' }
  },
  {
    path: 'resources/equipment',
    component: EquipmentDashboardComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'materials',
    loadComponent: () => import('./domains/materials/components/materials-hub/materials-hub.component')
      .then(c => c.MaterialsHubComponent),
    canActivate: [AuthGuard],
    data: { 
      breadcrumb: { label: 'Materials Hub', icon: 'inventory_2' }
    }
  },
  {
    path: 'materials/dashboard',
    component: MaterialInventoryDashboardComponent,
    canActivate: [AuthGuard],
    data: { 
      breadcrumb: { label: 'Inventory Dashboard', icon: 'dashboard' }
    }
  },
  {
    path: 'materials/catalog',
    component: MaterialsManagementComponent,
    canActivate: [AuthGuard],
    data: { 
      breadcrumb: { label: 'Material Catalog', icon: 'library_books' }
    }
  },
  {
    path: 'materials/catalog/list',
    component: MaterialsManagementComponent,
    canActivate: [AuthGuard],
    data: { 
      breadcrumb: { label: 'Material Catalog', icon: 'library_books' }
    }
  },
  {
    path: 'materials/work-order-hub',
    loadComponent: () => import('./domains/materials/components/work-order-material-hub/work-order-material-hub.component')
      .then(c => c.WorkOrderMaterialHubComponent),
    canActivate: [AuthGuard],
    data: { 
      breadcrumb: { label: 'Work Order Materials', icon: 'engineering' }
    }
  },
  {
    path: 'materials/purchase-orders',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: { 
      title: 'Purchase Orders', 
      message: 'Purchase order management functionality is coming soon.',
      breadcrumb: { label: 'Purchase Orders', icon: 'shopping_cart' }
    }
  },
  {
    path: 'materials/stock-movements',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: { 
      title: 'Stock Movements', 
      message: 'Stock movement tracking functionality is coming soon.',
      breadcrumb: { label: 'Stock Movements', icon: 'swap_horiz' }
    }
  },
  {
    path: 'materials/reports',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: { 
      title: 'Material Reports', 
      message: 'Material reporting functionality is coming soon.',
      breadcrumb: { label: 'Material Reports', icon: 'assessment' }
    }
  },

  // HR routes - protected
  {
    path: 'hr',
    loadChildren: () => import('./domains/hr/hr.module')
      .then(m => m.HrModule),
    canActivate: [AuthGuard]
  },

  // Reports routes - protected
  {
    path: 'reports',
    redirectTo: 'reports/monthly',
    pathMatch: 'full'
  },
  {
    path: 'reports/monthly',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: { title: 'Monthly Reports', message: 'The monthly reports page is currently under development.' }
  },
  {
    path: 'reports/performance',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: { title: 'Performance Reports', message: 'The performance reports page is currently under development.' }
  },
  {
    path: 'reports/financial',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: { title: 'Financial Reports', message: 'The financial reports page is currently under development.' }
  },

  // Users routes - protected
  {
    path: 'users',
    component: UserListComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'users/list',
    component: UserListComponent,
    canActivate: [AuthGuard]
  },

  // Settings routes - protected
  {
    path: 'settings',
    redirectTo: 'settings/general',
    pathMatch: 'full'
  },
  {
    path: 'settings/general',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: { title: 'General Settings', message: 'The general settings page is currently under development.' }
  },
  {
    path: 'settings/security',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: { title: 'Security Settings', message: 'The security settings page is currently under development.' }
  },
  {
    path: 'settings/notifications',
    component: UnderConstructionComponent,
    canActivate: [AuthGuard],
    data: { title: 'Notification Settings', message: 'The notification settings page is currently under development.' }
  },

  // Catch all route - redirect to dashboard
  {
    path: '**',
    redirectTo: 'dashboard/overview'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
