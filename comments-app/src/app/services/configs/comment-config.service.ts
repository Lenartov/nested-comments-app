import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CommentConfigService {
  readonly PAGE_SIZE = 25;
  readonly SORT_BY = 'createdAt';
  readonly SORT_DIR = 'desc';
  readonly DEFAULT_PAGE = 1;

  get apiUrl(): string {
    return `${environment.apiBaseUrl}/Comments`;
  }
}
