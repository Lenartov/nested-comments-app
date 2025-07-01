import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import type { CommentRead } from '../../models/comment.model';

@Component({
  selector: 'app-comment-list',
  standalone: true,
  imports: [CommonModule, CommentList], // рекурсія
  templateUrl: './comment-list.html',
  styleUrls: ['./comment-list.css'],
})
export class CommentList {
  @Input() comments: CommentRead[] = [];
}