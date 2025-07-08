import { Injectable } from '@angular/core';
import { HttpClient, HttpParams  } from '@angular/common/http';
import type { Observable } from 'rxjs';
import type { CommentRead, CommentListResponse } from '../models/comment.model';
import { CommentConfigService } from './configs/comment-config.service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(private http: HttpClient, private config: CommentConfigService) {}

  getComments(parentId?: number, pageNumber: number = this.config.DEFAULT_PAGE): Observable<CommentListResponse> {
    console.log('ENV:', environment.apiBaseUrl);
    const params = this.buildParams(parentId, pageNumber);
    return this.http.get<CommentListResponse>(this.config.apiUrl, { params });
  }

  private buildParams(parentId?: number, pageNumber: number = 1): HttpParams {
    let params = new HttpParams()
      .set('sortBy', 'CreatedAt')
      .set('sortDir', 'desc')
      .set('page', pageNumber)
      .set('pageSize', this.config.PAGE_SIZE);

    if (parentId !== undefined && parentId !== null) {
      params = params.set('parentId', parentId);
    }

    return params;
  }

  postComment(formData: FormData): Observable<CommentRead> {
  return this.http.post<CommentRead>(this.config.apiUrl, formData, { withCredentials: true });
}
}