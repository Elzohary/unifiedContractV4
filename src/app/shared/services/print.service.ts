import { Injectable } from '@angular/core';
import { WorkOrder, WorkOrderRemark, materialAssignment, PurchasableMaterial, ReceivableMaterial } from '../../domains/work-order/models/work-order.model';
import { ActivityLogService } from './activity-log.service';

@Injectable({
  providedIn: 'root'
})
export class PrintService {
  constructor(private activityLogService: ActivityLogService) {}

  /**
   * Prints the current page
   */
  printCurrentPage(): void {
    window.print();
  }

  /**
   * Prints a work order with a custom template
   * @param workOrder The work order to print
   * @param includeRemarks Whether to include remarks
   * @param includeActivityLog Whether to include activity log
   */
  printWorkOrder(
    workOrder: WorkOrder,
    includeRemarks = true,
    includeActivityLog = false
  ): void {
    // Log this activity
    this.activityLogService.addActivityLog({
      action: 'print',
      entityType: 'workOrder',
      entityId: workOrder.id.toString(),
      userId: 'system',
      userName: 'System',
      description: `Printed work order #${workOrder.details.workOrderNumber}`,
      details: {
        includeRemarks: includeRemarks.toString(),
        includeActivityLog: includeActivityLog.toString()
      }
    });

    // Create a new window for printing
    const printWindow = window.open('', '_blank');
    if (!printWindow) {
      alert('Please allow pop-ups to print this work order.');
      return;
    }

    // Format the date
    const formatDate = (date: Date | string | undefined): string => {
      if (!date) return 'N/A';
      return new Date(date).toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      });
    };

    // Generate remarks HTML if needed
    let remarksHtml = '';
    if (includeRemarks && workOrder.remarks && workOrder.remarks.length > 0) {
      remarksHtml = `
        <div class="print-section remarks-section">
          <h3>Remarks</h3>
          <div class="remarks-list">
            ${workOrder.remarks.map((remark: WorkOrderRemark) => `
              <div class="remark-item">
                <div class="remark-header">
                  <div class="remark-type ${remark.type.toLowerCase()}">${remark.type}</div>
                  <div class="remark-date">${formatDate(remark.createdDate)}</div>
                </div>
                <div class="remark-content">${remark.content}</div>
                <div class="remark-footer">
                  <div class="remark-author">By: ${remark.createdBy}</div>
                </div>
              </div>
            `).join('')}
          </div>
        </div>
      `;
    }

    // Generate HTML content
    const htmlContent = `
      <!DOCTYPE html>
      <html>
      <head>
        <title>Work Order #${workOrder.details.workOrderNumber}</title>
        <style>
          @media print {
            @page {
              size: A4;
              margin: 10mm;
            }
          }

          body {
            font-family: Arial, sans-serif;
            line-height: 1.5;
            color: #333;
            margin: 0;
            padding: 20px;
          }

          .print-header {
            text-align: center;
            margin-bottom: 30px;
            padding-bottom: 15px;
            border-bottom: 2px solid #eee;
          }

          .print-header h1 {
            font-size: 24px;
            margin: 0 0 10px;
          }

          .print-header p {
            font-size: 14px;
            color: #666;
            margin: 0;
          }

          .print-section {
            margin-bottom: 30px;
          }

          .print-section h2, .print-section h3 {
            margin-top: 0;
            color: #444;
            border-bottom: 1px solid #eee;
            padding-bottom: 8px;
          }

          .detail-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 15px;
          }

          .detail-item {
            margin-bottom: 10px;
          }

          .detail-label {
            font-weight: bold;
            display: block;
            margin-bottom: 3px;
            color: #666;
            font-size: 12px;
          }

          .detail-value {
            font-size: 14px;
          }

          .status {
            display: inline-block;
            padding: 5px 10px;
            border-radius: 4px;
            font-size: 12px;
            font-weight: bold;
            text-transform: uppercase;
          }

          .status-pending { background-color: #FFF3E0; color: #E65100; }
          .status-in-progress { background-color: #E3F2FD; color: #1565C0; }
          .status-completed { background-color: #E8F5E9; color: #2E7D32; }
          .status-cancelled { background-color: #EEEEEE; color: #757575; }

          .priority {
            display: inline-block;
            padding: 5px 10px;
            border-radius: 4px;
            font-size: 12px;
            font-weight: bold;
          }

          .priority-high { background-color: #FFEBEE; color: #C62828; }
          .priority-medium { background-color: #FFF8E1; color: #F9A825; }
          .priority-low { background-color: #E8F5E9; color: #2E7D32; }

          .completion-bar {
            height: 15px;
            background-color: #ECEFF1;
            border-radius: 8px;
            overflow: hidden;
            margin-top: 5px;
          }

          .completion-progress {
            height: 100%;
            background-color: #4CAF50;
          }

          .completion-label {
            text-align: right;
            font-size: 12px;
            margin-top: 3px;
            color: #666;
          }

          .info-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
          }

          .info-table th, .info-table td {
            padding: 10px;
            text-align: left;
            border-bottom: 1px solid #eee;
            font-size: 14px;
          }

          .info-table th {
            background-color: #f9f9f9;
            font-weight: bold;
            color: #666;
          }

          .info-table tr:last-child td {
            border-bottom: none;
          }

          .remarks-list {
            margin-top: 15px;
          }

          .remark-item {
            padding: 10px;
            border: 1px solid #eee;
            border-radius: 4px;
            margin-bottom: 10px;
          }

          .remark-header {
            display: flex;
            justify-content: space-between;
            margin-bottom: 10px;
            font-size: 12px;
          }

          .remark-type {
            font-weight: bold;
            padding: 3px 8px;
            border-radius: 4px;
          }

          .remark-type.note { background-color: #E3F2FD; color: #1565C0; }
          .remark-type.issue { background-color: #FFEBEE; color: #C62828; }
          .remark-type.feedback { background-color: #E8F5E9; color: #2E7D32; }

          .remark-date, .remark-author {
            color: #666;
          }

          .remark-content {
            padding: 5px 0;
            font-size: 14px;
          }

          .remark-footer {
            font-size: 12px;
            color: #666;
            text-align: right;
            margin-top: 5px;
          }

          .site-report-item {
            background-color: #fafafa;
            border-left: 4px solid #2196F3 !important;
          }

          .site-report-item h4 {
            color: #2196F3;
            font-size: 16px;
            font-weight: 600;
          }

          .status-open { background-color: #E3F2FD; color: #1565C0; }
          .status-resolved { background-color: #E8F5E9; color: #2E7D32; }
          .status-closed { background-color: #EEEEEE; color: #757575; }

          .page-break {
            page-break-before: always;
          }

          .section-break {
            margin-top: 30px;
            padding-top: 20px;
            border-top: 2px solid #eee;
          }

          .footer {
            margin-top: 50px;
            text-align: center;
            font-size: 12px;
            color: #999;
            padding-top: 20px;
            border-top: 1px solid #eee;
          }
        </style>
      </head>
      <body>
        <div class="print-header">
          <h1>Work Order #${workOrder.details.workOrderNumber}</h1>
          <p>Generated on ${new Date().toLocaleString()}</p>
        </div>

        <div class="print-section">
          <h2>${workOrder.details.title}</h2>
          <p>${workOrder.details.description}</p>

          <div class="detail-grid">
            <div class="detail-item">
              <span class="detail-label">Status</span>
              <div class="status status-${workOrder.details.status.toLowerCase()}">${workOrder.details.status}</div>
            </div>

            <div class="detail-item">
              <span class="detail-label">Priority</span>
              <div class="priority priority-${workOrder.details.priority.toLowerCase()}">${workOrder.details.priority}</div>
            </div>
          </div>
        </div>

        <div class="print-section">
          <h3>Work Order Details</h3>
          <div class="detail-grid">
            <div class="detail-item">
              <span class="detail-label">Created Date</span>
              <div class="detail-value">${formatDate(workOrder.details.createdDate)}</div>
            </div>

            <div class="detail-item">
              <span class="detail-label">Start Date</span>
              <div class="detail-value">${formatDate(workOrder.details.startDate)}</div>
            </div>

            <div class="detail-item">
              <span class="detail-label">Due Date</span>
              <div class="detail-value">${formatDate(workOrder.details.targetEndDate || workOrder.details.dueDate)}</div>
            </div>

            <div class="detail-item">
              <span class="detail-label">Category</span>
              <div class="detail-value">${workOrder.details.category}</div>
            </div>

            <div class="detail-item">
              <span class="detail-label">Type</span>
              <div class="detail-value">${workOrder.details.type || 'N/A'}</div>
            </div>

            <div class="detail-item">
              <span class="detail-label">Class</span>
              <div class="detail-value">${workOrder.details.class || 'N/A'}</div>
            </div>

            <div class="detail-item">
              <span class="detail-label">Project Type</span>
              <div class="detail-value">${workOrder.details.projectType || 'N/A'}</div>
            </div>

            <div class="detail-item">
              <span class="detail-label">PO</span>
              <div class="detail-value">${workOrder.details.po || 'N/A'}</div>
            </div>

            <div class="detail-item">
              <span class="detail-label">D1</span>
              <div class="detail-value">${workOrder.details.d1 || 'N/A'}</div>
            </div>

            <div class="detail-item">
              <span class="detail-label">Internal Order Number</span>
              <div class="detail-value">${workOrder.details.internalOrderNumber || 'N/A'}</div>
            </div>

            <div class="detail-item">
              <span class="detail-label">Estimated Cost</span>
              <div class="detail-value">$${workOrder.details['estimatedCost']?.toFixed(2) || '0.00'}</div>
            </div>

            <div class="detail-item" style="margin-top: 15px;">
              <span class="detail-label">Completion</span>
              <div class="completion-bar">
                <div class="completion-progress" style="width: ${workOrder.details.completionPercentage}%;"></div>
              </div>
              <div class="completion-label">${workOrder.details.completionPercentage}%</div>
            </div>
          </div>
        </div>

        <div class="print-section">
          <h3>Client Information</h3>
          <div class="detail-grid">
            <div class="detail-item">
              <span class="detail-label">Client Name</span>
              <div class="detail-value">${workOrder.details.client}</div>
            </div>

            <div class="detail-item">
              <span class="detail-label">Location</span>
              <div class="detail-value">${workOrder.details.location}</div>
            </div>
          </div>
        </div>

        <div class="print-section">
          <h3>Team Information</h3>
          <div class="detail-item">
            <span class="detail-label">Engineer In Charge</span>
            <div class="detail-value">${workOrder.engineerInCharge?.name || 'Not Assigned'}</div>
          </div>

          <div class="detail-item">
            <span class="detail-label">Team Members</span>
            <div class="detail-value">
              ${workOrder.manpower && workOrder.manpower.length > 0
                ? workOrder.manpower.map(m => m.name || m.badgeNumber).join(', ')
                : 'No team members assigned'}
            </div>
          </div>
        </div>

        ${workOrder.materials && workOrder.materials.length > 0 ? `
        <div class="print-section">
          <h3>Materials</h3>
          <table class="info-table">
            <thead>
              <tr>
                <th>Material</th>
                <th>Quantity</th>
                <th>Unit</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              ${workOrder.materials.map((material: materialAssignment) => {
                const materialData = material.materialType === 'purchasable' ? material.purchasableMaterial : material.receivableMaterial;
                const quantity = material.materialType === 'purchasable'
                  ? (materialData as PurchasableMaterial).quantity
                  : (materialData as ReceivableMaterial).estimatedQuantity;
                return `
                  <tr>
                    <td>${materialData?.name || 'N/A'}</td>
                    <td>${quantity || '0'}</td>
                    <td>${materialData?.unit || 'N/A'}</td>
                    <td>${materialData?.status || 'N/A'}</td>
                  </tr>
                `;
              }).join('')}
            </tbody>
          </table>
        </div>
        ` : ''}

        ${workOrder.items && workOrder.items.length > 0 ? `
        <div class="print-section">
          <h3>Work Order Items</h3>
          <table class="info-table">
            <thead>
              <tr>
                <th>Item Number</th>
                <th>Description</th>
                <th>UOM</th>
                <th>Estimated Qty</th>
                <th>Unit Price</th>
                <th>Estimated Price</th>
                <th>Management Area</th>
              </tr>
            </thead>
            <tbody>
              ${workOrder.items.map((item: any) => `
                <tr>
                  <td>${item.itemNumber || 'N/A'}</td>
                  <td>${item.shortDescription || 'N/A'}</td>
                  <td>${item.UOM || 'N/A'}</td>
                  <td>${item.estimatedQuantity || '0'}</td>
                  <td>$${item.unitPrice?.toFixed(2) || '0.00'}</td>
                  <td>$${item.estimatedPrice?.toFixed(2) || '0.00'}</td>
                  <td>${item.managementArea || 'N/A'}</td>
                </tr>
              `).join('')}
            </tbody>
          </table>
        </div>
        ` : ''}

        ${workOrder.permits && workOrder.permits.length > 0 ? `
        <div class="print-section">
          <h3>Permits</h3>
          <table class="info-table">
            <thead>
              <tr>
                <th>Type</th>
                <th>Status</th>
                <th>Number</th>
                <th>Authority</th>
                <th>Issue Date</th>
                <th>Expiry Date</th>
                <th>Issued By</th>
              </tr>
            </thead>
            <tbody>
              ${workOrder.permits.map((permit: any) => `
                <tr>
                  <td>${permit.type || 'N/A'}</td>
                  <td>
                    <span class="status status-${permit.status?.toLowerCase() || 'pending'}">${permit.status || 'Pending'}</span>
                  </td>
                  <td>${permit.number || 'N/A'}</td>
                  <td>${permit.authority || 'N/A'}</td>
                  <td>${formatDate(permit.issueDate)}</td>
                  <td>${formatDate(permit.expiryDate)}</td>
                  <td>${permit.issuedBy || 'N/A'}</td>
                </tr>
              `).join('')}
            </tbody>
          </table>
        </div>
        ` : ''}

        ${workOrder.siteReports && workOrder.siteReports.length > 0 ? `
        <div class="print-section">
          <h3>Site Reports</h3>
          ${workOrder.siteReports.map((report: any, index: number) => `
            <div class="site-report-item" style="margin-bottom: 20px; padding: 15px; border: 1px solid #eee; border-radius: 4px;">
              <h4 style="margin: 0 0 10px 0; color: #333;">Site Report #${index + 1}</h4>
              <div class="detail-grid">
                <div class="detail-item">
                  <span class="detail-label">Report Date</span>
                  <div class="detail-value">${formatDate(report.reportDate)}</div>
                </div>
                <div class="detail-item">
                  <span class="detail-label">Status</span>
                  <div class="detail-value">
                    <span class="status status-${report.status?.toLowerCase() || 'pending'}">${report.status || 'Pending'}</span>
                  </div>
                </div>
                <div class="detail-item">
                  <span class="detail-label">Weather Conditions</span>
                  <div class="detail-value">${report.weatherConditions || 'N/A'}</div>
                </div>
                <div class="detail-item">
                  <span class="detail-label">Work Progress</span>
                  <div class="detail-value">${report.workProgress || 'N/A'}</div>
                </div>
              </div>
              ${report.observations ? `
                <div style="margin-top: 10px;">
                  <span class="detail-label">Observations</span>
                  <div class="detail-value">${report.observations}</div>
                </div>
              ` : ''}
              ${report.recommendations ? `
                <div style="margin-top: 10px;">
                  <span class="detail-label">Recommendations</span>
                  <div class="detail-value">${report.recommendations}</div>
                </div>
              ` : ''}
            </div>
          `).join('')}
        </div>
        ` : ''}

        ${workOrder.tasks && workOrder.tasks.length > 0 ? `
        <div class="print-section">
          <h3>Tasks</h3>
          <table class="info-table">
            <thead>
              <tr>
                <th>Task</th>
                <th>Status</th>
                <th>Assigned To</th>
                <th>Due Date</th>
                <th>Priority</th>
              </tr>
            </thead>
            <tbody>
              ${workOrder.tasks.map((task: any) => `
                <tr>
                  <td>${task.title || 'N/A'}</td>
                  <td>
                    <span class="status status-${task.status?.toLowerCase() || 'pending'}">${task.status || 'Pending'}</span>
                  </td>
                  <td>${task.assignedTo || 'N/A'}</td>
                  <td>${formatDate(task.dueDate)}</td>
                  <td>${task.priority || 'Normal'}</td>
                </tr>
              `).join('')}
            </tbody>
          </table>
        </div>
        ` : ''}

        ${workOrder.issues && workOrder.issues.length > 0 ? `
        <div class="print-section">
          <h3>Issues</h3>
          <table class="info-table">
            <thead>
              <tr>
                <th>Issue</th>
                <th>Status</th>
                <th>Priority</th>
                <th>Reported By</th>
                <th>Reported Date</th>
                <th>Resolution</th>
              </tr>
            </thead>
            <tbody>
              ${workOrder.issues.map((issue: any) => `
                <tr>
                  <td>${issue.title || 'N/A'}</td>
                  <td>
                    <span class="status status-${issue.status?.toLowerCase() || 'open'}">${issue.status || 'Open'}</span>
                  </td>
                  <td>${issue.priority || 'Normal'}</td>
                  <td>${issue.reportedBy || 'N/A'}</td>
                  <td>${formatDate(issue.reportedDate)}</td>
                  <td>${issue.resolution || 'N/A'}</td>
                </tr>
              `).join('')}
            </tbody>
          </table>
        </div>
        ` : ''}

        ${remarksHtml}

        <div class="footer">
          <p>This document is automatically generated and does not require a signature.</p>
          <p>Printed on ${new Date().toLocaleDateString()} at ${new Date().toLocaleTimeString()}</p>
        </div>
      </body>
      </html>
    `;

    // Write to the new window and print
    printWindow.document.open();
    printWindow.document.write(htmlContent);
    printWindow.document.close();

    // Wait for resources to load before printing
    printWindow.addEventListener('load', () => {
      // Slight delay to ensure CSS is applied
      setTimeout(() => {
        printWindow.print();
        // Close the window after printing (or if user cancels)
        printWindow.addEventListener('afterprint', () => {
          printWindow.close();
        });
      }, 500);
    });
  }

  /**
   * Prints a work order in O & M WORK ORDER FORM format
   * @param workOrder The work order to print
   */
  printWorkOrderForm(workOrder: WorkOrder): void {
    console.log('🖨️ Printing O & M WORK ORDER FORM for:', workOrder.details.workOrderNumber);
    // Log this activity
    this.activityLogService.addActivityLog({
      action: 'print',
      entityType: 'workOrder',
      entityId: workOrder.id.toString(),
      userId: 'system',
      userName: 'System',
      description: `Printed work order form #${workOrder.details.workOrderNumber}`,
      details: {
        format: 'O&M_FORM'
      }
    });

    // Create a new window for printing
    const printWindow = window.open('', '_blank');
    if (!printWindow) {
      alert('Please allow pop-ups to print this work order form.');
      return;
    }

    // Format the date
    const formatDate = (date: Date | string | undefined): string => {
      if (!date) return '';
      return new Date(date).toLocaleDateString('en-US', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
      });
    };

    // Calculate totals
    const calculateLaborTotal = () => {
      if (!workOrder.items || workOrder.items.length === 0) return 0;
      return workOrder.items.reduce((total, item) => total + (item.estimatedPrice || 0), 0);
    };

    const laborTotal = calculateLaborTotal();
    const vatAmount = laborTotal * 0.15; // 15% VAT
    const totalWithVAT = laborTotal + vatAmount;

    // Generate materials HTML - only receivable materials
    let materialsHtml = '';
    if (workOrder.materials && workOrder.materials.length > 0) {
      const receivableMaterials = workOrder.materials.filter(material => material.materialType === 'receivable');
      if (receivableMaterials.length > 0) {
        materialsHtml = `
          <table class="materials-table">
            <thead>
              <tr>
                <th>SL#</th>
                <th>ITEM#</th>
                <th>MATERIALS DESCRIPTION</th>
                <th>QTY</th>
                <th>UNIT</th>
                <th>REMARKS</th>
              </tr>
            </thead>
            <tbody>
              ${receivableMaterials.map((material, index) => {
                const materialData = material.receivableMaterial as ReceivableMaterial;
                return `
                  <tr>
                    <td>${index + 1}</td>
                    <td>${materialData?.id || 'N/A'}</td>
                    <td>${materialData?.name || 'N/A'}</td>
                    <td>${materialData?.estimatedQuantity || '0'}</td>
                    <td>${materialData?.unit || 'N/A'}</td>
                    <td></td>
                  </tr>
                `;
              }).join('')}
            </tbody>
          </table>
        `;
      }
    }

    // Generate HTML content
    const htmlContent = `
      <!DOCTYPE html>
      <html>
      <head>
        <title>O & M WORK ORDER FORM - ${workOrder.details.workOrderNumber}</title>
        <style>
          @media print {
            @page {
              size: A4;
              margin: 15mm;
            }
          }

          body {
            font-family: Arial, sans-serif;
            line-height: 1.4;
            color: #000;
            margin: 0;
            padding: 0;
            background: white;
          }

          .form-container {
            max-width: 210mm;
            margin: 0 auto;
            padding: 20px;
          }

          .header {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            margin-bottom: 30px;
            border-bottom: 2px solid #000;
            padding-bottom: 15px;
          }

          .logo-section {
            flex: 0 0 200px;
            display: flex;
            align-items: center;
          }

          .logo-section img {
            max-width: 160px;
            height: auto;
          }

          .company-info {
            flex: 1;
            text-align: center;
            display: flex;
            flex-direction: column;
            justify-content: center;
          }

          .form-title {
            font-size: 28px;
            font-weight: bold;
            text-transform: uppercase;
            margin: 0 0 8px 0;
            text-align: center;
            letter-spacing: 1px;
          }

          .company-name {
            font-size: 14px;
            color: #333;
            margin: 0 0 4px 0;
            font-weight: 500;
          }

          .company-tagline {
            font-size: 12px;
            color: #666;
            margin: 0;
            font-style: italic;
          }

          .contractor-info {
            flex: 0 0 280px;
            text-align: right;
            font-size: 11px;
            line-height: 1.3;
          }

          .contractor-info div {
            margin-bottom: 3px;
          }

          .contractor-info .project-title {
            font-weight: bold;
            font-size: 12px;
            margin: 8px 0 4px 0;
            color: #000;
          }

          .contractor-info .work-order-number {
            font-size: 16px;
            font-weight: bold;
            margin: 4px 0;
            color: #000;
          }

          .contractor-info .date-info {
            font-size: 10px;
            color: #333;
          }

          .work-order-details {
            margin-bottom: 20px;
          }

          .work-order-details table {
            width: 100%;
            border-collapse: collapse;
          }

          .work-order-details td {
            padding: 5px 10px;
            border: 1px solid #ccc;
            font-size: 12px;
          }

          .work-order-details td:first-child {
            font-weight: bold;
            background-color: #f5f5f5;
            width: 120px;
          }

          .labor-cost-section {
            margin-bottom: 20px;
          }

          .section-title {
            font-size: 18px;
            font-weight: bold;
            text-transform: uppercase;
            margin-bottom: 12px;
            text-align: center;
            background-color: #e0e0e0;
            padding: 10px;
            border: 2px solid #000;
            letter-spacing: 1px;
          }

          .labor-table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 15px;
          }

          .labor-table th,
          .labor-table td {
            border: 1px solid #000;
            padding: 8px;
            text-align: left;
            font-size: 11px;
            vertical-align: top;
          }

          .labor-table th {
            background-color: #e0e0e0;
            font-weight: bold;
            text-align: center;
            font-size: 12px;
            padding: 10px 8px;
          }

          .labor-table .description-cell {
            width: 40%;
          }

          .labor-table .number-cell {
            width: 10%;
            text-align: center;
          }

          .labor-table .unit-cell {
            width: 8%;
            text-align: center;
          }

          .labor-table .qty-cell {
            width: 8%;
            text-align: center;
          }

          .labor-table .rate-cell {
            width: 12%;
            text-align: right;
          }

          .labor-table .amount-cell {
            width: 12%;
            text-align: right;
          }

          .total-row {
            font-weight: bold;
            background-color: #f9f9f9;
          }

          .status-section {
            margin-bottom: 20px;
          }

          .status-box {
            border: 2px solid #000;
            min-height: 100px;
            padding: 10px;
            margin-top: 10px;
          }

          .materials-section {
            margin-bottom: 20px;
          }

          .materials-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
          }

          .materials-table th,
          .materials-table td {
            border: 1px solid #000;
            padding: 6px;
            text-align: left;
            font-size: 11px;
          }

          .materials-table th {
            background-color: #f0f0f0;
            font-weight: bold;
            text-align: center;
          }

          .reference-section {
            margin-bottom: 20px;
          }

          .reference-table {
            width: 100%;
            border-collapse: collapse;
          }

          .reference-table td {
            border: 1px solid #ccc;
            padding: 8px;
            font-size: 12px;
            height: 30px;
          }

          .reference-table td:first-child {
            font-weight: bold;
            background-color: #f5f5f5;
            width: 150px;
          }

          .signature-section {
            display: flex;
            justify-content: space-between;
            margin-top: 30px;
          }

          .signature-box {
            flex: 1;
            margin: 0 10px;
            text-align: center;
          }

          .signature-line {
            border-bottom: 1px solid #000;
            height: 40px;
            margin-bottom: 5px;
          }

          .signature-label {
            font-size: 12px;
            font-weight: bold;
            margin-bottom: 5px;
          }

          .signature-name {
            font-size: 11px;
            color: #666;
          }

          .stamp-section {
            text-align: right;
            margin-top: 20px;
          }

          .stamp {
            display: inline-block;
            border: 2px solid #000;
            border-radius: 50%;
            width: 80px;
            height: 80px;
            text-align: center;
            font-size: 8px;
            padding: 10px;
            color: #000;
          }
        </style>
      </head>
      <body>
        <div class="form-container">
          <!-- Header -->
          <div class="header">
            <div class="logo-section">
              <img src="/assets/clientLogo.png" alt="Saudi Electricity Company" />
            </div>
            <div class="company-info">
              <div class="form-title">O & M WORK ORDER FORM</div>
              <div class="company-name">الشركة السعودية للكهرباء</div>
              <div class="company-tagline">طاقة مثمرة</div>
            </div>
            <div class="contractor-info">
              <div><strong>CONTRACTOR:</strong> Excellence Tracks</div>
              <div><strong>VENDOR NO.:</strong> 1000178</div>
              <div><strong>PROJECT NO.:</strong> 1-2543002.21.0001</div>
              <div class="project-title">${workOrder.details.title || 'REPLACE METER BOX'}</div>
              <div class="work-order-number">${workOrder.details.workOrderNumber}</div>
              <div class="date-info"><strong>ISSUED DATE:</strong> ${formatDate(workOrder.details.createdDate)}</div>
              <div class="date-info"><strong>TARGET DATE:</strong> ${formatDate(workOrder.details.targetEndDate || workOrder.details.dueDate)}</div>
            </div>
          </div>

          <!-- Work Order Details -->
          <div class="work-order-details">
            <table>
              <tr>
                <td style="width: 120px; font-weight: bold; background-color: #f5f5f5;">LOCATION:</td>
                <td style="width: 150px;">${workOrder.details.location || 'HAFAR'}</td>
                <td style="width: 80px; font-weight: bold; background-color: #f5f5f5;">S/S#:</td>
                <td style="width: 100px;"></td>
                <td style="width: 140px; font-weight: bold; background-color: #f5f5f5;">WORK ORDER NO:</td>
                <td style="width: 150px;">${workOrder.details.workOrderNumber}</td>
              </tr>
            </table>
          </div>

          <!-- Labor Cost Section -->
          <div class="labor-cost-section">
            <div class="section-title">LABOR COST</div>
            <table class="labor-table">
              <thead>
                <tr>
                  <th>ITEM NO</th>
                  <th class="description-cell">DESCRIPTION</th>
                  <th class="unit-cell">UNIT</th>
                  <th class="qty-cell">QTY</th>
                  <th class="rate-cell">RATE</th>
                  <th class="amount-cell">AMOUNT</th>
                </tr>
              </thead>
              <tbody>
                ${workOrder.items && workOrder.items.length > 0 ? workOrder.items.map((item, index) => `
                  <tr>
                    <td class="number-cell">${item.itemDetail?.itemNumber || 'N/A'}</td>
                    <td class="description-cell">${item.itemDetail?.shortDescription || 'N/A'}<br/>
                      <span style="font-size: 10px; color: #666;">${item.itemDetail?.longDescription || ''}</span>
                    </td>
                    <td class="unit-cell">${item.itemDetail?.UOM || 'N/A'}</td>
                    <td class="qty-cell">${item.estimatedQuantity || '0'}</td>
                    <td class="rate-cell">${item.itemDetail?.unitPrice?.toFixed(2) || '0.00'}</td>
                    <td class="amount-cell">${item.estimatedPrice?.toFixed(2) || '0.00'}</td>
                  </tr>
                `).join('') : `
                  <tr>
                    <td class="number-cell">502010001</td>
                    <td class="description-cell">Replacement of an existing one whole current-meter assembly by another one whole current meter assembly with its accessories, including all disconnections and reconnections works, Any size / type<br/>
                      <span style="font-size: 10px; color: #666;">استبدال مجموعة عداد تيار كاملة موجودة بمجموعة عداد تيار كاملة أخرى مع ملحقاتها، بما في ذلك جميع أعمال الفصل وإعادة الاتصال، أي حجم / نوع</span>
                    </td>
                    <td class="unit-cell">ASM</td>
                    <td class="qty-cell">100</td>
                    <td class="rate-cell">230.00</td>
                    <td class="amount-cell">23,000.00</td>
                  </tr>
                  <tr>
                    <td class="number-cell">305010401</td>
                    <td class="description-cell">installation of terminal lugs (4EA) for 4-Cores LV cable, any size for an existing equipment / meter<br/>
                      <span style="font-size: 10px; color: #666;">تركيب وصلات طرفية (4 قطع) لكابل الجهد المنخفض 4 نوى، أي حجم للمعدات / العداد الموجود</span>
                    </td>
                    <td class="unit-cell">ST</td>
                    <td class="qty-cell">200</td>
                    <td class="rate-cell">113.00</td>
                    <td class="amount-cell">22,600.00</td>
                  </tr>
                `}
                <tr class="total-row">
                  <td colspan="5" style="text-align: right;"><strong>TOTAL:</strong></td>
                  <td class="amount-cell"><strong>${laborTotal.toFixed(2)}</strong></td>
                </tr>
                <tr class="total-row">
                  <td colspan="5" style="text-align: right;"><strong>45%:</strong></td>
                  <td class="amount-cell"><strong>0.00</strong></td>
                </tr>
                <tr class="total-row">
                  <td colspan="5" style="text-align: right;"><strong>TOTAL:</strong></td>
                  <td class="amount-cell"><strong>${laborTotal.toFixed(2)}</strong></td>
                </tr>
              </tbody>
            </table>
          </div>

          <!-- Status Section -->
          <div class="status-section">
            <div class="section-title">STATUS</div>
            <div class="status-box">
              <!-- Status content will be added here -->
            </div>
          </div>

          <!-- Materials Section -->
          <div class="materials-section">
            <div class="section-title">MATERIALS USED / (Specified Who Provide Materials SCECO or Contractor on Remarks.)</div>
            ${materialsHtml}
            <div style="text-align: center; margin-top: 10px; font-weight: bold;">************MATERIALS USED IN SITE************</div>
          </div>

          <!-- Reference Numbers Section -->
          <div class="reference-section">
            <table class="reference-table">
              <tr>
                <td>NOTIFICATION NO.</td>
                <td></td>
                <td>RESERVATION NO.</td>
                <td></td>
              </tr>
              <tr>
                <td>W.O. NO.</td>
                <td>${workOrder.details.workOrderNumber}</td>
                <td></td>
                <td></td>
              </tr>
              <tr>
                <td>PURCHASE REQUISITION</td>
                <td></td>
                <td></td>
                <td></td>
              </tr>
              <tr>
                <td>PURCHASE ORDER</td>
                <td></td>
                <td></td>
                <td></td>
              </tr>
              <tr>
                <td>ENTRY SHEET</td>
                <td></td>
                <td></td>
                <td></td>
              </tr>
            </table>
          </div>

          <!-- Signature Section -->
          <div class="signature-section">
            <div class="signature-box">
              <div class="signature-label">PREPARED BY:</div>
              <div class="signature-line"></div>
              <div class="signature-name"></div>
            </div>
            <div class="signature-box">
              <div class="signature-label">VERIFIED BY:</div>
              <div class="signature-line"></div>
              <div class="signature-name"></div>
            </div>
            <div class="signature-box">
              <div class="signature-label">SCECO REPRESENTATIVE:</div>
              <div class="signature-line"></div>
              <div class="signature-name"></div>
            </div>
          </div>
        </div>
      </body>
      </html>
    `;

    // Write to the new window and print
    printWindow.document.open();
    printWindow.document.write(htmlContent);
    printWindow.document.close();

    // Wait for resources to load before printing
    printWindow.addEventListener('load', () => {
      // Slight delay to ensure CSS is applied
      setTimeout(() => {
        printWindow.print();
        // Close the window after printing (or if user cancels)
        printWindow.addEventListener('afterprint', () => {
          printWindow.close();
        });
      }, 500);
    });
  }

  /**
   * Prints a specific element with custom CSS
   * @param elementId The ID of the element to print
   * @param title The title of the print document
   * @param additionalStyles Additional CSS styles to apply
   */
  printElement(elementId: string, title: string, additionalStyles = ''): void {
    const element = document.getElementById(elementId);
    if (!element) {
      console.error(`Element with ID ${elementId} not found`);
      return;
    }

    const printWindow = window.open('', '_blank');
    if (!printWindow) {
      alert('Please allow pop-ups to print this content.');
      return;
    }

    const htmlContent = `
      <!DOCTYPE html>
      <html>
      <head>
        <title>${title}</title>
        <style>
          @media print {
            @page {
              size: A4;
              margin: 10mm;
            }
          }

          body {
            font-family: Arial, sans-serif;
            line-height: 1.5;
            color: #333;
            margin: 0;
            padding: 20px;
          }

          ${additionalStyles}
        </style>
      </head>
      <body>
        <div class="print-container">
          ${element.outerHTML}
        </div>
      </body>
      </html>
    `;

    printWindow.document.open();
    printWindow.document.write(htmlContent);
    printWindow.document.close();

    printWindow.addEventListener('load', () => {
      setTimeout(() => {
        printWindow.print();
        printWindow.addEventListener('afterprint', () => {
          printWindow.close();
        });
      }, 500);
    });
  }
}