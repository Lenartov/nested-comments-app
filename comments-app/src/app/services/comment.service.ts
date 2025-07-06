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

  getComments(parentId: number = -1, pageNumber: number = 1): Observable<CommentListResponse> {
    let params = new HttpParams();
    if(parentId <= 0)
    {
      params = this.getMainCommentsParams(pageNumber);
    }
    else
    {
      params = this.getRepliesParams(parentId, pageNumber);
    }
    return this.http.get<CommentListResponse>(this.apiUrl, {params});
  }

  getMainCommentsParams(pageNumber: number = 1): HttpParams{
      const params = new HttpParams()
      .set('sortBy', 'CreatedAt')
      .set('sortDir', 'desc')
      .set('page', pageNumber)
      .set('pageSize', 5);

      return params;
  }

    getRepliesParams(parentId: number, pageNumber: number = 1): HttpParams{
      const params = new HttpParams()
      .set('parentId', parentId)
      .set('sortBy', 'CreatedAt')
      .set('sortDir', 'desc')
      .set('page', pageNumber)
      .set('pageSize', 5);

      return params;
  }

  postComment(formData: FormData) {
  return this.http.post<CommentRead>(this.apiUrl, formData, { withCredentials: true });
}
}