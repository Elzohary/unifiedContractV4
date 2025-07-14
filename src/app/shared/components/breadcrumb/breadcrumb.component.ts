import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, NavigationEnd, ActivatedRoute, RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Subject } from 'rxjs';
import { takeUntil, filter } from 'rxjs/operators';

export interface BreadcrumbItem {
  label: string;
  url?: string;
  icon?: string;
  disabled?: boolean;
}

@Component({
  selector: 'app-breadcrumb',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule, MatButtonModule, MatTooltipModule],
  template: `
    <nav class="breadcrumb-nav" *ngIf="breadcrumbs.length > 0">
      <ol class="breadcrumb-list">
        <li class="breadcrumb-item" *ngFor="let item of breadcrumbs; let last = last">
          <ng-container *ngIf="!last; else lastItem">
            <button mat-button 
                    class="breadcrumb-link"
                    [routerLink]="item.url"
                    [disabled]="item.disabled"
                    [matTooltip]="item.label"
                    *ngIf="item.url; else noLink">
              <mat-icon *ngIf="item.icon" class="breadcrumb-icon">{{ item.icon }}</mat-icon>
              <span class="breadcrumb-text">{{ item.label }}</span>
            </button>
            <ng-template #noLink>
              <span class="breadcrumb-text">
                <mat-icon *ngIf="item.icon" class="breadcrumb-icon">{{ item.icon }}</mat-icon>
                {{ item.label }}
              </span>
            </ng-template>
          </ng-container>
          <ng-template #lastItem>
            <span class="breadcrumb-text current">
              <mat-icon *ngIf="item.icon" class="breadcrumb-icon">{{ item.icon }}</mat-icon>
              {{ item.label }}
            </span>
          </ng-template>
          <mat-icon *ngIf="!last" class="breadcrumb-separator">chevron_right</mat-icon>
        </li>
      </ol>
    </nav>
  `,
  styleUrls: ['./breadcrumb.component.scss']
})
export class BreadcrumbComponent implements OnInit, OnDestroy {
  breadcrumbs: BreadcrumbItem[] = [];
  private destroy$ = new Subject<void>();

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.buildBreadcrumbs();
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private buildBreadcrumbs(): void {
    const breadcrumbs: BreadcrumbItem[] = [];
    let currentRoute = this.activatedRoute.root;
    let url = '';

    while (currentRoute.children.length > 0) {
      const childrenRoutes = currentRoute.children;
      let breadcrumbRoute = null;

      for (const route of childrenRoutes) {
        if (route.outlet === 'primary') {
          const routeSnapshot = route.snapshot;
          url += '/' + routeSnapshot.url.map(segment => segment.path).join('/');

          const breadcrumbData = route.snapshot.data['breadcrumb'];
          if (breadcrumbData) {
            breadcrumbRoute = {
              label: breadcrumbData.label || route.snapshot.data['title'] || 'Unknown',
              url: url,
              icon: breadcrumbData.icon,
              disabled: breadcrumbData.disabled
            };
          }
          currentRoute = route;
          break;
        }
      }

      if (breadcrumbRoute) {
        breadcrumbs.push(breadcrumbRoute);
      }
    }

    // Add home breadcrumb if not already present
    if (breadcrumbs.length === 0 || breadcrumbs[0].label !== 'Home') {
      breadcrumbs.unshift({
        label: 'Home',
        url: '/',
        icon: 'home'
      });
    }

    this.breadcrumbs = breadcrumbs;
  }

  // Method to manually set breadcrumbs (for complex scenarios)
  setBreadcrumbs(breadcrumbs: BreadcrumbItem[]): void {
    this.breadcrumbs = breadcrumbs;
  }
}
