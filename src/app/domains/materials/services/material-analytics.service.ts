import { Injectable, inject } from '@angular/core';
import { Observable, combineLatest, of } from 'rxjs';
import { map, switchMap, catchError } from 'rxjs/operators';
import { HttpParams } from '@angular/common/http';
import { MockDatabaseService } from '../../../core/services/mock-database.service';
import { WorkOrderService } from '../../work-order/services/work-order.service';
import { MaterialService } from './material.service';
import { environment } from '../../../../environments/environment';
import { ApiService } from '../../../core/services/api.service';

// Analytics Data Interfaces
export interface MaterialAnalyticsData {
  costAnalysis: CostAnalysisData;
  efficiencyMetrics: EfficiencyMetricsData;
  usageTrends: UsageTrendsData;
  allocationSummary: AllocationSummaryData;
  performanceIndicators: PerformanceIndicatorsData;
}

export interface CostAnalysisData {
  budgetVariance: number;
  averageCostPerWorkOrder: number;
  totalEstimatedCost: number;
  totalActualCost: number;
  costSavings: number;
  costOverruns: number;
  costTrend: 'increasing' | 'decreasing' | 'stable';
}

export interface EfficiencyMetricsData {
  utilizationRate: number;
  wastePercentage: number;
  allocationEfficiency: number;
  deliveryEfficiency: number;
  materialTurnover: number;
}

export interface UsageTrendsData {
  monthlyUsage: MonthlyUsageData[];
  materialConsumption: MaterialConsumptionData[];
  seasonalPatterns: SeasonalPatternData[];
  forecastData: ForecastData[];
}

export interface MonthlyUsageData {
  month: string;
  totalUsed: number;
  totalAllocated: number;
  efficiency: number;
}

export interface MaterialConsumptionData {
  materialId: string;
  materialCode: string;
  materialName: string;
  totalConsumed: number;
  averageMonthlyUsage: number;
  trend: 'increasing' | 'decreasing' | 'stable';
}

export interface SeasonalPatternData {
  season: string;
  averageUsage: number;
  peakUsage: number;
  lowUsage: number;
}

export interface ForecastData {
  period: string;
  predictedUsage: number;
  confidenceLevel: number;
  factors: string[];
}

export interface AllocationSummaryData {
  totalMaterials: number;
  totalAllocated: number;
  totalUsed: number;
  totalRemaining: number;
  allocationRate: number;
  usageRate: number;
  pendingDeliveries: number;
  overdueDeliveries: number;
}

export interface PerformanceIndicatorsData {
  kpis: KPIData[];
  alerts: AlertData[];
  recommendations: RecommendationData[];
}

export interface KPIData {
  name: string;
  value: number;
  target: number;
  unit: string;
  status: 'excellent' | 'good' | 'warning' | 'critical';
  trend: 'up' | 'down' | 'stable';
}

export interface AlertData {
  id: string;
  type: 'cost' | 'efficiency' | 'allocation' | 'delivery';
  severity: 'low' | 'medium' | 'high' | 'critical';
  message: string;
  timestamp: Date;
  actionable: boolean;
}

export interface RecommendationData {
  id: string;
  category: 'cost' | 'efficiency' | 'allocation' | 'procurement';
  title: string;
  description: string;
  impact: 'low' | 'medium' | 'high';
  effort: 'low' | 'medium' | 'high';
  priority: number;
}

@Injectable({
  providedIn: 'root'
})
export class MaterialAnalyticsService {
  private mockDb = inject(MockDatabaseService);
  private workOrderService = inject(WorkOrderService);
  private materialService = inject(MaterialService);
  private apiService = inject(ApiService);

  private readonly analyticsEndpoint = 'material-analytics';

  /**
   * Get comprehensive analytics data
   */
  getAnalyticsData(): Observable<MaterialAnalyticsData> {
    if (environment.useMockData) {
      return this.getMockAnalyticsData();
    } else {
      return this.getApiAnalyticsData();
    }
  }

  /**
   * Get cost analysis data
   */
  getCostAnalysis(): Observable<CostAnalysisData> {
    if (environment.useMockData) {
      return this.calculateMockCostAnalysis();
    } else {
      return this.apiService.get<CostAnalysisData>(`${this.analyticsEndpoint}/cost-analysis`).pipe(
        map(response => response.data || response as any)
      );
    }
  }

  /**
   * Get efficiency metrics
   */
  getEfficiencyMetrics(): Observable<EfficiencyMetricsData> {
    if (environment.useMockData) {
      return this.calculateMockEfficiencyMetrics();
    } else {
      return this.apiService.get<EfficiencyMetricsData>(`${this.analyticsEndpoint}/efficiency-metrics`).pipe(
        map(response => response.data || response as any)
      );
    }
  }

  /**
   * Get usage trends data
   */
  getUsageTrends(): Observable<UsageTrendsData> {
    if (environment.useMockData) {
      return this.generateMockUsageTrends();
    } else {
      return this.apiService.get<UsageTrendsData>(`${this.analyticsEndpoint}/usage-trends`).pipe(
        map(response => response.data || response as any)
      );
    }
  }

  /**
   * Get allocation summary
   */
  getAllocationSummary(): Observable<AllocationSummaryData> {
    if (environment.useMockData) {
      return this.calculateMockAllocationSummary();
    } else {
      return this.apiService.get<AllocationSummaryData>(`${this.analyticsEndpoint}/allocation-summary`).pipe(
        map(response => response.data || response as any)
      );
    }
  }

  /**
   * Get performance indicators
   */
  getPerformanceIndicators(): Observable<PerformanceIndicatorsData> {
    if (environment.useMockData) {
      return this.generateMockPerformanceIndicators();
    } else {
      return this.apiService.get<PerformanceIndicatorsData>(`${this.analyticsEndpoint}/performance-indicators`).pipe(
        map(response => response.data || response as any)
      );
    }
  }

  /**
   * Get analytics data for specific date range
   */
  getAnalyticsDataForPeriod(startDate: Date, endDate: Date): Observable<MaterialAnalyticsData> {
    if (environment.useMockData) {
      return this.getMockAnalyticsData();
    } else {
      const params = new HttpParams()
        .set('startDate', startDate.toISOString())
        .set('endDate', endDate.toISOString());
      
      return this.apiService.get<MaterialAnalyticsData>(`${this.analyticsEndpoint}/period`, params).pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Export analytics report
   */
  exportAnalyticsReport(format: 'pdf' | 'excel' | 'csv'): Observable<Blob> {
    if (environment.useMockData) {
      // Mock export - return empty blob
      return of(new Blob(['Mock analytics report'], { type: 'text/plain' }));
    } else {
      const params = new HttpParams().set('format', format);
      
      return this.apiService.get<Blob>(`${this.analyticsEndpoint}/export`, params).pipe(
        map(response => response.data)
      );
    }
  }

  // Private methods for mock data generation

  private getMockAnalyticsData(): Observable<MaterialAnalyticsData> {
    return combineLatest([
      this.calculateMockCostAnalysis(),
      this.calculateMockEfficiencyMetrics(),
      this.generateMockUsageTrends(),
      this.calculateMockAllocationSummary(),
      this.generateMockPerformanceIndicators()
    ]).pipe(
      map(([costAnalysis, efficiencyMetrics, usageTrends, allocationSummary, performanceIndicators]) => ({
        costAnalysis,
        efficiencyMetrics,
        usageTrends,
        allocationSummary,
        performanceIndicators
      }))
    );
  }

  private getApiAnalyticsData(): Observable<MaterialAnalyticsData> {
    return this.apiService.get<MaterialAnalyticsData>(this.analyticsEndpoint).pipe(
      map(response => response.data),
      catchError(error => {
        console.error('Error fetching analytics data from API:', error);
        // Fallback to mock data
        return this.getMockAnalyticsData();
      })
    );
  }

  private calculateMockCostAnalysis(): Observable<CostAnalysisData> {
    return this.workOrderService.getAllWorkOrders().pipe(
      map(workOrders => {
        const totalEstimated = workOrders.reduce((sum, wo) => {
          const materials = wo.materials || [];
          return sum + materials.reduce((matSum, mat) => {
            if (mat.purchasableMaterial) {
              return matSum + (mat.purchasableMaterial.totalCost || 0);
            }
            return matSum;
          }, 0);
        }, 0);

        const totalActual = workOrders.reduce((sum, wo) => {
          const materials = wo.materials || [];
          return sum + materials.reduce((matSum, mat) => {
            if (mat.purchasableMaterial && mat.purchasableMaterial.status === 'used') {
              return matSum + (mat.purchasableMaterial.totalCost || 0);
            }
            return matSum;
          }, 0);
        }, 0);

        const budgetVariance = totalEstimated > 0 ? ((totalActual - totalEstimated) / totalEstimated) * 100 : 0;
        const averageCostPerWorkOrder = workOrders.length > 0 ? totalActual / workOrders.length : 0;
        const costSavings = Math.max(0, totalEstimated - totalActual);
        const costOverruns = Math.max(0, totalActual - totalEstimated);

        return {
          budgetVariance,
          averageCostPerWorkOrder,
          totalEstimatedCost: totalEstimated,
          totalActualCost: totalActual,
          costSavings,
          costOverruns,
          costTrend: budgetVariance > 5 ? 'increasing' : budgetVariance < -5 ? 'decreasing' : 'stable'
        };
      })
    );
  }

  private calculateMockEfficiencyMetrics(): Observable<EfficiencyMetricsData> {
    return combineLatest([
      this.workOrderService.getAllWorkOrders(),
      this.materialService.materials$
    ]).pipe(
      map(([workOrders, materials]) => {
        const totalAssigned = workOrders.reduce((sum, wo) => {
          const materials = wo.materials || [];
          return sum + materials.length;
        }, 0);

        const totalUsed = workOrders.reduce((sum, wo) => {
          const materials = wo.materials || [];
          return sum + materials.filter(m => 
            m.purchasableMaterial?.status === 'used' || 
            m.receivableMaterial?.status === 'used'
          ).length;
        }, 0);

        const totalDelivered = workOrders.reduce((sum, wo) => {
          const materials = wo.materials || [];
          return sum + materials.filter(m => 
            m.purchasableMaterial?.status === 'delivered' || 
            m.receivableMaterial?.status === 'received'
          ).length;
        }, 0);

        const utilizationRate = totalAssigned > 0 ? (totalUsed / totalAssigned) * 100 : 0;
        const wastePercentage = totalDelivered > 0 ? ((totalDelivered - totalUsed) / totalDelivered) * 100 : 0;
        const allocationEfficiency = materials.length > 0 ? (totalAssigned / materials.length) * 100 : 0;
        const deliveryEfficiency = totalAssigned > 0 ? (totalDelivered / totalAssigned) * 100 : 0;
        const materialTurnover = totalUsed > 0 ? (totalUsed / materials.length) : 0;

        return {
          utilizationRate,
          wastePercentage,
          allocationEfficiency,
          deliveryEfficiency,
          materialTurnover
        };
      })
    );
  }

  private generateMockUsageTrends(): Observable<UsageTrendsData> {
    // Generate mock monthly usage data for the last 12 months
    const monthlyUsage: MonthlyUsageData[] = [];
    const currentDate = new Date();
    
    for (let i = 11; i >= 0; i--) {
      const date = new Date(currentDate.getFullYear(), currentDate.getMonth() - i, 1);
      const month = date.toLocaleDateString('en-US', { month: 'short', year: 'numeric' });
      const totalUsed = Math.floor(Math.random() * 1000) + 500;
      const totalAllocated = Math.floor(Math.random() * 1200) + 600;
      const efficiency = totalAllocated > 0 ? (totalUsed / totalAllocated) * 100 : 0;
      
      monthlyUsage.push({ month, totalUsed, totalAllocated, efficiency });
    }

    // Generate mock material consumption data
    return this.materialService.materials$.pipe(
      map(materials => {
        const materialConsumption: MaterialConsumptionData[] = materials.slice(0, 10).map(material => ({
          materialId: material.id!,
          materialCode: material.code,
          materialName: material.description,
          totalConsumed: Math.floor(Math.random() * 500) + 100,
          averageMonthlyUsage: Math.floor(Math.random() * 50) + 10,
          trend: Math.random() > 0.5 ? 'increasing' : Math.random() > 0.5 ? 'decreasing' : 'stable'
        }));

        const seasonalPatterns: SeasonalPatternData[] = [
          { season: 'Spring', averageUsage: 800, peakUsage: 1200, lowUsage: 600 },
          { season: 'Summer', averageUsage: 1000, peakUsage: 1400, lowUsage: 800 },
          { season: 'Fall', averageUsage: 900, peakUsage: 1100, lowUsage: 700 },
          { season: 'Winter', averageUsage: 700, peakUsage: 900, lowUsage: 500 }
        ];

        const forecastData: ForecastData[] = monthlyUsage.slice(-3).map((usage, index) => ({
          period: `Month ${index + 1}`,
          predictedUsage: usage.totalUsed * (1 + (Math.random() * 0.2 - 0.1)),
          confidenceLevel: 85 + Math.random() * 10,
          factors: ['Seasonal demand', 'Project completion', 'Market conditions']
        }));

        return {
          monthlyUsage,
          materialConsumption,
          seasonalPatterns,
          forecastData
        };
      })
    );
  }

  private calculateMockAllocationSummary(): Observable<AllocationSummaryData> {
    return combineLatest([
      this.workOrderService.getAllWorkOrders(),
      this.materialService.materials$
    ]).pipe(
      map(([workOrders, materials]) => {
        const totalMaterials = materials.length;
        const totalAllocated = workOrders.reduce((sum, wo) => sum + (wo.materials?.length || 0), 0);
        const totalUsed = workOrders.reduce((sum, wo) => {
          const materials = wo.materials || [];
          return sum + materials.filter(m => 
            m.purchasableMaterial?.status === 'used' || 
            m.receivableMaterial?.status === 'used'
          ).length;
        }, 0);
        const totalRemaining = totalAllocated - totalUsed;
        const allocationRate = totalMaterials > 0 ? (totalAllocated / totalMaterials) * 100 : 0;
        const usageRate = totalAllocated > 0 ? (totalUsed / totalAllocated) * 100 : 0;
        const pendingDeliveries = Math.floor(Math.random() * 20) + 5;
        const overdueDeliveries = Math.floor(Math.random() * 5);

        return {
          totalMaterials,
          totalAllocated,
          totalUsed,
          totalRemaining,
          allocationRate,
          usageRate,
          pendingDeliveries,
          overdueDeliveries
        };
      })
    );
  }

  private generateMockPerformanceIndicators(): Observable<PerformanceIndicatorsData> {
    const kpis: KPIData[] = [
      {
        name: 'Material Utilization Rate',
        value: 78.5,
        target: 85,
        unit: '%',
        status: 'good',
        trend: 'up'
      },
      {
        name: 'Cost Variance',
        value: -2.3,
        target: 0,
        unit: '%',
        status: 'excellent',
        trend: 'down'
      },
      {
        name: 'Delivery Efficiency',
        value: 92.1,
        target: 90,
        unit: '%',
        status: 'excellent',
        trend: 'up'
      },
      {
        name: 'Waste Percentage',
        value: 8.5,
        target: 5,
        unit: '%',
        status: 'warning',
        trend: 'down'
      }
    ];

    const alerts: AlertData[] = [
      {
        id: 'alert-001',
        type: 'cost',
        severity: 'medium',
        message: 'Cost variance exceeded 5% threshold for Project WO-2024-001',
        timestamp: new Date(),
        actionable: true
      },
      {
        id: 'alert-002',
        type: 'efficiency',
        severity: 'low',
        message: 'Material utilization rate below target for electrical materials',
        timestamp: new Date(Date.now() - 86400000),
        actionable: true
      }
    ];

    const recommendations: RecommendationData[] = [
      {
        id: 'rec-001',
        category: 'cost',
        title: 'Optimize Material Procurement',
        description: 'Consider bulk purchasing for frequently used materials to reduce unit costs',
        impact: 'high',
        effort: 'medium',
        priority: 1
      },
      {
        id: 'rec-002',
        category: 'efficiency',
        title: 'Improve Allocation Planning',
        description: 'Implement better forecasting to reduce over-allocation of materials',
        impact: 'medium',
        effort: 'high',
        priority: 2
      }
    ];

    return of({
      kpis,
      alerts,
      recommendations
    });
  }
} 