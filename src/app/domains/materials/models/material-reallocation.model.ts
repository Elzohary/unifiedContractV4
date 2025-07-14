export type MaterialReallocationStatus = 'pending' | 'approved' | 'rejected' | 'completed';

export interface MaterialReallocation {
  id: string;
  materialId: string;
  fromWorkOrderId: string;
  toWorkOrderId: string;
  quantity: number;
  reason: string;
  requestedBy: string;
  requestedAt: Date;
  status: MaterialReallocationStatus;
  approvedBy?: string;
  approvedAt?: Date;
  rejectedBy?: string;
  rejectedAt?: Date;
  completedAt?: Date;
  errorMessage?: string;
  auditTrail: MaterialReallocationAudit[];
}

export interface MaterialReallocationAudit {
  id: string;
  action: 'requested' | 'approved' | 'rejected' | 'completed' | 'rollback' | 'error';
  performedBy: string;
  performedAt: Date;
  notes?: string;
} 