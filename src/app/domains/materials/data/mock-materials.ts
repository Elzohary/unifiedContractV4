import { BaseMaterial, ClientType, MaterialType, SecMaterial } from '../models/material.model';

// SEC materials mock data
export const mockSecMaterials: SecMaterial[] = [
  {
    id: '1',
    code: 'SEC-001',
    description: 'Electrical Meter Type A',
    unit: 'PCS',
    materialType: MaterialType.RECEIVABLE,
    clientType: ClientType.SEC,
    groupCode: 'ELM',
    groupCodeDescription: 'Electrical Meters',
    SEQ: 1001,
    materialMasterCode: 'SEC-ELM-001'
  },
  {
    id: '2',
    code: 'SEC-002',
    description: 'Cable 16mm',
    unit: 'M',
    materialType: MaterialType.RECEIVABLE,
    clientType: ClientType.SEC,
    groupCode: 'ELC',
    groupCodeDescription: 'Electrical Cables',
    SEQ: 2001,
    materialMasterCode: 'SEC-ELC-001'
  },
  {
    id: '3',
    code: 'SEC-003',
    description: 'Junction Box Type B',
    unit: 'PCS',
    materialType: MaterialType.RECEIVABLE,
    clientType: ClientType.SEC,
    groupCode: 'ELJ',
    groupCodeDescription: 'Electrical Junctions',
    SEQ: 3001,
    materialMasterCode: 'SEC-ELJ-001'
  }
];

// Add more purchasable materials
export const mockPurchasableMaterials: BaseMaterial[] = [
  {
    id: 'p1',
    code: 'PUR-001',
    description: 'Cement',
    unit: 'BAG',
    materialType: MaterialType.PURCHASABLE,
    clientType: ClientType.OTHER,
    status: 'in-use',
    totalStock: 100,
    availableStock: 80
  },
  {
    id: 'p2',
    code: 'PUR-002',
    description: 'Steel Bars',
    unit: 'TON',
    materialType: MaterialType.PURCHASABLE,
    clientType: ClientType.OTHER,
    status: 'ordered',
    totalStock: 50,
    availableStock: 30
  },
  {
    id: 'p3',
    code: 'PUR-003',
    description: 'Ready Mix Concrete',
    unit: 'M3',
    materialType: MaterialType.PURCHASABLE,
    clientType: ClientType.OTHER,
    status: 'in-use',
    totalStock: 200,
    availableStock: 150
  }
];

// Add more receivable materials
export const mockReceivableMaterials: BaseMaterial[] = [
  {
    id: 'r1',
    code: 'REC-001',
    description: 'Electrical Meter Type A',
    unit: 'PCS',
    materialType: MaterialType.RECEIVABLE,
    clientType: ClientType.SEC,
    status: 'received',
    totalStock: 60,
    availableStock: 55
  },
  {
    id: 'r2',
    code: 'REC-002',
    description: 'Cable 16mm',
    unit: 'M',
    materialType: MaterialType.RECEIVABLE,
    clientType: ClientType.SEC,
    status: 'pending',
    totalStock: 100,
    availableStock: 90
  },
  {
    id: 'r3',
    code: 'REC-003',
    description: 'Junction Box Type B',
    unit: 'PCS',
    materialType: MaterialType.RECEIVABLE,
    clientType: ClientType.SEC,
    status: 'received',
    totalStock: 40,
    availableStock: 35
  }
];

// Combined mock data
export const mockMaterials: BaseMaterial[] = [
  ...mockSecMaterials,
  ...mockPurchasableMaterials,
  ...mockReceivableMaterials
];
