import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class FileService {
  private readonly fileBaseUrl = 'http://localhost:5237';

  getFullUrl(filePath: string): string {
    return `${this.fileBaseUrl}${filePath}`;
  }

  isImage(filePath: string): boolean {
    return /\.(jpg|jpeg|png|gif|)$/i.test(this.getFullUrl(filePath));
  }

  openFile(filePath: string): void {
      window.open(this.getFullUrl(filePath), '_blank');
  }
}