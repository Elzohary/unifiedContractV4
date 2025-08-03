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