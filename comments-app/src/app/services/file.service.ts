import { Injectable } from '@angular/core';
import { FileConfigService } from './file-config.service';

@Injectable({
  providedIn: 'root',
})
export class FileService {
  constructor(private config: FileConfigService) {}

  getFullUrl(filePath: string): string {
    return `${this.config.baseUrl}${filePath}`;
  }

  isImage(filePath: string): boolean {
    const regex = new RegExp(`\\.(${this.config.imageExtensions.join('|')})$`, 'i');
    return regex.test(filePath);  }

  openFile(filePath: string): void {
      window.open(this.getFullUrl(filePath), '_blank');
  }
}