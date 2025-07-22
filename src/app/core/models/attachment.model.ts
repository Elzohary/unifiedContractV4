export interface Attachment {
    id: string;
    fileName: string;
    fileType: string;
    fileSize: number;
    filePath: string;
    uploadedById: string;
    uploadDate: Date;
    entityId: string;
    entityType: string;
    createdBy: string;
    createdAt: Date;
} 