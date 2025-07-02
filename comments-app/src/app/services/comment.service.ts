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

  getComments(sortBy: string = 'CreatedAt', sortDir: string = 'desc', page: number = 1, pageSize: number = 25): Observable<CommentListResponse> {

        const params = new HttpParams()
      .set('sortBy', sortBy)
      .set('sortDir', sortDir)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<CommentListResponse>(this.apiUrl);
  }
  
    getReplies(parentId: number): Observable<CommentListResponse> {
    const params = new HttpParams().set('parentId', parentId.toString());
    return this.http.get<CommentListResponse>(this.apiUrl, { params });
  }

  postComment(formData: FormData) {
  return this.http.post<CommentRead>(this.apiUrl, formData);
}
}