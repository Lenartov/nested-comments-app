import { Component, Input, OnInit } from '@angular/core';
import { CommentRead, CommentListResponse } from '../../models/comment.model';
import { CommentService } from '../../services/comment.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-comment-list',
      imports: [CommonModule],

  templateUrl: './comment-list.html',
  styleUrls: ['./comment-list.css'],
  standalone: true,
})

export class CommentList implements OnInit {
  @Input() comments: CommentRead[] = [];

  // Картка з дочірніми коментарями (id батька => replies)
  commentRepliesMap: { [parentId: number]: CommentRead[] } = {};

  // Множина id коментарів, які розгорнуті для показу відповідей
  expandedComments = new Set<number>();

  // Параметри сортування
  sortBy: string = 'createdAt';
  sortDir: 'asc' | 'desc' = 'desc';

  constructor(private commentService: CommentService) {}

  ngOnInit(): void {
    // Якщо хочеш, можна тут ініціалізувати або додатково завантажити щось
  }

  // Перевірити, чи розгорнутий коментар
  isExpanded(comment: CommentRead): boolean {
    return this.expandedComments.has(comment.id);
  }

  // Показати / приховати відповіді
  toggleReplies(comment: CommentRead): void {
    if (this.isExpanded(comment)) {
      this.expandedComments.delete(comment.id);
    } else {
      this.expandedComments.add(comment.id);
      // Якщо ще не завантажені відповіді — завантажуємо
      if (!this.commentRepliesMap[comment.id]) {
        this.loadReplies(comment);
      }
    }
  }

  // Завантаження відповідей по API
  loadReplies(comment: CommentRead): void {
    this.commentService.getReplies(comment.id).subscribe({
      next: (replies) => {
        this.commentRepliesMap[comment.id] = replies.items;
      },
      error: (err) => {
        console.error('Failed to load replies', err);
        this.commentRepliesMap[comment.id] = [];
      }
    });
  }
  
sortField: string = '';
sortDirection: 'asc' | 'desc' = 'asc';

sort(field: keyof CommentRead) {
  if (this.sortField === field) {
    this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
  } else {
    this.sortField = field;
    this.sortDirection = 'asc';
  }

  this.comments.sort((a, b) => {
    let valA = a[field];
    let valB = b[field];

    let compareA: any = valA;
    let compareB: any = valB;

    if (field === 'createdAt') {
      compareA = typeof valA === 'string' ? new Date(valA) : new Date(0);
      compareB = typeof valB === 'string' ? new Date(valB) : new Date(0);
    }

    if (compareA == null) return 1;
    if (compareB == null) return -1;

    if (compareA < compareB) return this.sortDirection === 'asc' ? -1 : 1;
    if (compareA > compareB) return this.sortDirection === 'asc' ? 1 : -1;
    return 0;
  });
}




  // Перезавантаження списку з новим сортуванням (припускаємо, що кореневі коментарі підтягуємо заново)
  loadSortedComments(): void {
    this.commentService.getComments(this.sortBy, this.sortDir, 1, 25).subscribe({
      next: (data) => {
        this.comments = data.items;
        this.expandedComments.clear();
        this.commentRepliesMap = {};
      },
      error: (err) => {
        console.error('Failed to load comments', err);
      }
    });
  }
}