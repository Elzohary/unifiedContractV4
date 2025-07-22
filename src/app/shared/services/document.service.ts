import { Injectable } from '@angular/core';
import { ApiService, ApiResponse } from 'src/app/core/services/api.service';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { Attachment } from 'src/app/core/models/attachment.model';

export interface UploadedDocument {
  id: string;
  fileName: string;
  fileUrl: string;
  fileType: string;
  fileSize: number;
  uploadedDate: Date;
  uploadedBy: string; // Should be user ID or name
  entityType: string;
  entityId: string;
  file?: File;
}

@Injectable({
  providedIn: 'root'
})
export class DocumentService {

  constructor(private apiService: ApiService) { }

  getDocumentsByEntity(entityType: string, entityId: string): Observable<UploadedDocument[]> {
    return this.apiService.get<Attachment[]>(`/documents/${entityType}/${entityId}`).pipe(
      map(response => {
        if (!response.data) return [];
        return response.data.map(this.mapAttachmentToUploadedDocument);
      })
    );
  }

  deleteDocument(documentId: string): Observable<boolean> {
    return this.apiService.delete(`/documents/${documentId}`).pipe(
      map(response => response.status === 200)
    );
  }

  uploadDocument(file: File, entityId: string, entityType: string): Observable<UploadedDocument> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    formData.append('entityId', entityId);
    formData.append('entityType', entityType);
    
    return this.apiService.post<{ filePath: string, id: string }>('/documents/upload', formData).pipe(
      map(response => {
        if (!response.data) {
          throw new Error("Invalid response from server");
        }
        const newDoc: UploadedDocument = {
          id: response.data.id,
          fileName: file.name,
          fileUrl: response.data.filePath,
          fileType: file.type,
          fileSize: file.size,
          uploadedDate: new Date(),
          uploadedBy: 'current_user', // This should be replaced with actual user data
          entityId: entityId,
          entityType: entityType,
          file: file
        };
        return newDoc;
      })
    );
  }

  validateFile(file: File, allowedTypes?: string[], maxSizeMB?: number): { valid: boolean; error?: string } {
    if (allowedTypes && allowedTypes.length > 0) {
      const fileExtension = file.name.split('.').pop()?.toLowerCase();
      const isValidType = allowedTypes.some(type => {
        if (type.endsWith('/*') && file.type.startsWith(type.slice(0, -1))) {
          return true;
        }
        return file.type === type || `.${fileExtension}` === type;
      });
      
      if (!isValidType) {
        return { valid: false, error: `File type not allowed. Allowed types: ${allowedTypes.join(', ')}` };
      }
    }
    
    if (maxSizeMB) {
      const maxSizeBytes = maxSizeMB * 1024 * 1024;
      if (file.size > maxSizeBytes) {
        return { valid: false, error: `File size exceeds ${maxSizeMB}MB limit.` };
      }
    }
    
    return { valid: true };
  }

  private mapAttachmentToUploadedDocument(attachment: Attachment): UploadedDocument {
    return {
      id: attachment.id,
      fileName: attachment.fileName,
      fileUrl: attachment.filePath,
      fileType: attachment.fileType,
      fileSize: attachment.fileSize,
      uploadedDate: attachment.createdAt,
      uploadedBy: attachment.createdBy,
      entityId: attachment.entityId.toString(),
      entityType: attachment.entityType
    };
  }
} 