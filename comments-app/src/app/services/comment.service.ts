import { Injectable } from '@angular/core';
import { HttpClient, HttpParams  } from '@angular/common/http';
import type { Observable } from 'rxjs';
import type { CommentCreate, CommentRead, CommentListResponse } from '../models/comment.model';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private readonly apiUrl = 'http://localhost:5237/api/Comments';

  constructor(private http: HttpClient) {}

  getComments(page: number = 1): Observable<CommentListResponse> {

        const params = new HttpParams()
      .set('sortBy', 'CreatedAt')
      .set('sortDir', 'desc')
      .set('page', page.toString())
      .set('pageSize', 5);

    return this.http.get<CommentListResponse>(this.apiUrl, {params});
  }
  
    getReplies(parentId: number, page: number = 1): Observable<CommentListResponse> {
      const params = new HttpParams()
      .set('parentId', parentId.toString())
      .set('sortBy', 'CreatedAt')
      .set('sortDir', 'desc')
      .set('page', page.toString())
      .set('pageSize', 5);

    return this.http.get<CommentListResponse>(this.apiUrl, { params , withCredentials: true });
  }

  postComment(formData: FormData) {
  return this.http.post<CommentRead>(this.apiUrl, formData, { withCredentials: true });
}
}