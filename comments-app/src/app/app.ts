import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { provideHttpClient, withFetch  } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { CommentService } from './services/comment.service';
import { CommentForm } from './components/comment-form/comment-form';
import { CommentList } from './components/comment-list/comment-list';

import { CommentRead } from './models/comment.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommentForm, CommentList, CommonModule],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App {
  comments: CommentRead[] = [];

  constructor(private http: HttpClient, private commentService: CommentService) {}

ngOnInit() {
    this.commentService.getComments().subscribe(data => {
      this.comments = data;
    });
  }}
  
  /*ngOnInit() {
    this.commentService.getComments().subscribe(data => {
      this.comments = data;
        console.log('Отримані коментарі:', this.comments);

    });
  }*/
    
