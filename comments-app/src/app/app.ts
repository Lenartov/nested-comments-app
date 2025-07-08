import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { CommentService } from './services/comment.service';
import { CommentFormComponent } from './components/comment-form/comment-form.component';
import { CommentList } from './components/comment-list/comment-list';
import { SignalRService } from './services/signalr.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommentFormComponent, CommentList, CommonModule],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App {
  constructor(private http: HttpClient, private commentService: CommentService, private signalRService: SignalRService) {}

  ngOnInit(): void {
    this.signalRService.startConnection();
}
}

