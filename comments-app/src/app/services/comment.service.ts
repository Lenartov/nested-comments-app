import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import type { Observable } from 'rxjs';
import type { CommentCreate, CommentRead } from '../models/comment.model';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private readonly apiUrl = 'http://localhost:5237/api/Comments';

  constructor(private http: HttpClient) {}

  getComments(): Observable<CommentRead[]> {
    return this.http.get<CommentRead[]>(this.apiUrl);
  }

  postComment(formData: FormData) {
  return this.http.post<CommentRead>(this.apiUrl, formData);
}
}