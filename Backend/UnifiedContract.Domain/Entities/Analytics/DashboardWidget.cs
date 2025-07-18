using System;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Entities.Analytics
{
    public class DashboardWidget : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // Chart, KPI, Table, List, etc.
        public string ChartType { get; set; } // Bar, Line, Pie, etc.
        public string DataSource { get; set; } // API endpoint or query identifier
        public string Configuration { get; set; } // JSON configuration for the widget
        public int Width { get; set; } // Grid width (1-12)
        public int Height { get; set; } // Grid height in pixels
        public int Position { get; set; } // Order in the dashboard
        public bool IsActive { get; set; }
        public Guid DashboardId { get; set; }
        
        // Navigation properties
        public virtual Dashboard Dashboard { get; set; }
    }
} 