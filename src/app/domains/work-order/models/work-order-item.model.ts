export interface Iitem {
  id: string;
  itemNumber: string;
  lineType: 'Description' | 'Breakdown';
  shortDescription: string;
  longDescription: string;
  UOM: string;
  currency: string;
  unitPrice: number;
  paymentType:string;
  managementArea:string;
  clientId?: string;
  isActive?: boolean;
}

// New interfaces for the many-to-many relationship
export interface ItemDto {
  id: string;
  itemNumber: string;
  description: string;
  unit: string;
  unitPrice: number;
  paymentType: string;
  managementArea: string;
  currency: string;
  isActive: boolean;
  clientId: string;
  clientName: string;
  createdAt: string;
  createdBy: string;
  lastModifiedAt: string;
  lastModifiedBy: string;
}

export interface WorkOrderItemAssignmentDto {
  id: string;
  workOrderId: string;
  itemId: string;
  estimatedQuantity: number;
  estimatedPrice: number;
  estimatedPriceWithVAT: number;
  actualQuantity: number;
  actualPrice: number;
  actualPriceWithVAT: number;
  reasonForFinalQuantity: string;
  createdAt: string;
  createdBy: string;
  lastModifiedAt: string;
  lastModifiedBy: string;
  item: ItemDto | null;
}

export interface estimatedItemsPricesDetails {
  totalEstimatedPrice: number;
  EstimationVAT: number;
  totalEstimatedPriceWithVAT: number;
  partialPaymentTotalEstimated: number;
  partialPaymentVAT: number;
  partialPaymentTotalEstimatedWithVAT: number;
}

export interface actualItemsPricesDetails {
  totalActualPrice: number;
  actualVAT: number;
  totalActualPriceWithVAT: number;
  partialPaymentTotalActual: number;
  partialPaymentVAT: number;
  partialPaymentTotalActualWithVAT: number;
}

export interface finalItemsPricesDetails {
  totalFinalPrice: number;
  finalVAT: number;
  totalFinalPriceWithVAT: number;
  partialPaymentTotalFinal: number;
  partialPaymentFinalVAT: number;
  partialPaymentTotalFinalWithVAT: number;
}
